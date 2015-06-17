/*******************************************************************************
 *******************************************************************************
            Author: Simon Bridge, June 2011 mailto:srbridge@gmail.com
 
 
        This code is provided under the Code Project Open Licence (CPOL)
          See http://www.codeproject.com/info/cpol10.aspx for details
  
 *******************************************************************************
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;

namespace Vietbait.NetworkMonitor
{
    /// <summary>
    /// monitors network status and raises events when the status of a network interface changes.
    /// </summary>
    public class NetworkStatusMonitor
    {
        #region Fields

        /// <summary>
        /// pulse to signal WaitForPoll.
        /// </summary>
        private readonly object _pulse = new object();

        /// <summary>
        /// number of exceptions logged.
        /// </summary>
        private int _exceptionCount;

        /// <summary>
        /// started flag.
        /// </summary>
        private bool _isStarted;

        /// <summary>
        /// last recorded network interface status.
        /// </summary>
        private NetworkStatus _last;

        /// <summary>
        /// flag for any-status-change events.
        /// </summary>
        private bool _monitorAnyStatusChange;

        /// <summary>
        /// flag for dis-connection events.
        /// </summary>
        private bool _monitorDisconnections = true;

        /// <summary>
        /// flag for new connection events.
        /// </summary>
        private bool _monitorNewConnections = true;

        /// <summary>
        /// flag for IP change.
        /// </summary>
        private bool _monitorNetworkAddressChanged = true;

        /// <summary>
        /// thread monitoring network status and raising events.
        /// </summary>
        private Thread _monitorThread;

        /// <summary>
        /// run flag.
        /// </summary>
        private bool _run = true;

        /// <summary>
        /// the wait interval for the monitor.
        /// </summary>
        private int _waitInterval = 100;

        #endregion

        #region Properties

        /// <summary>
        /// gets or sets the polling interval.
        /// </summary>
        public int PollInterval
        {
            get { return _waitInterval; }
            set { _waitInterval = value; }
        }

        /// <summary>
        /// gets if the status monitor is started.
        /// </summary>
        public bool IsStarted
        {
            get { return _isStarted; }
        }

        /// <summary>
        /// gets or sets the flag to monitor interface connections.
        /// </summary>
        public bool MonitorNewConnections
        {
            get { return _monitorNewConnections; }
            set { _monitorNewConnections = value; }
        }

        /// <summary>
        /// gets or sets the flag to monitor interface dis-connections.
        /// </summary>
        public bool MonitorDisconnections
        {
            get { return _monitorDisconnections; }
            set { _monitorDisconnections = value; }
        }

        /// <summary>
        /// gets or sets the flag to monitor for any status changes.
        /// </summary>
        public bool MonitorAnyChanges
        {
            get { return _monitorAnyStatusChange; }
            set { _monitorAnyStatusChange = value; }
        }

        /// <summary>
        /// gets or sets the flag to monitor for any Ip change.
        /// </summary>
        public bool MonitorNetworkAddressChanged
        {
            get { return _monitorNetworkAddressChanged; }
            set { _monitorNetworkAddressChanged = value; }
        }

        /// <summary>
        /// gets the number of exceptions (if any) that have been logged since the
        /// connection monitor started.
        /// </summary>
        public int ExceptionCount
        {
            get { return _exceptionCount; }
        }

        #endregion

        #region Events

        /// <summary>
        /// event raised when a network interface is connected.
        /// </summary>
        public event EventHandler<StatusMonitorEventArgs> NetworkInterfaceConnected;

        /// <summary>
        /// event raised when a network interface is disconnected.
        /// </summary>
        public event EventHandler<StatusMonitorEventArgs> NetworkInterfaceDisconnected;

        /// <summary>
        /// event raised when a network interfaces operational-status changes.
        /// </summary>
        public event EventHandler<StatusMonitorEventArgs> NetworkInterfaceChanged;


        /// <summary>
        /// event raised when a network interfaces operational-Address changes.
        /// </summary>
        public event EventHandler<StatusMonitorEventArgs> NetworkAddressChanged;



        #endregion

        #region Constructor

        /// <summary>
        /// construct the network status monitor with the given poll interval.
        /// </summary>
        /// <param name="pollInterval"></param>
        public NetworkStatusMonitor(int pollInterval)
        {
            // create the monitor thread;
            _monitorThread = new Thread(MonitorTask);
            try
            {
                _monitorThread.Name = "NetMon";
            }
            catch
            {
            }
            // assign the wait interval.
            _waitInterval = pollInterval;
        }

        #endregion

        #region Start and Stop

        /// <summary>
        /// command to start monitoring and raising events.
        /// </summary>
        public void StartMonitoring()
        {
            if (!_isStarted)
            {
                _run = true;
                _monitorThread.Start();
                _isStarted = true;
            }
        }

        /// <summary>
        /// command to stop the monitor thread.
        /// </summary>
        public void StopMonitoring()
        {
            if (_isStarted)
            {
                _run = false;
                _monitorThread.Join();
                _monitorThread = new Thread(MonitorTask);
                _isStarted = false;
            }
        }

        #endregion

        #region Thread Monitor Task

        /// <summary>
        /// block the calling thread until a poll interval is reached.
        /// </summary>
        public void WaitForPoll()
        {
            // this shouldn't be called from the monitor thread 
            // just return if it is.
            if (_monitorThread == Thread.CurrentThread)
                return;

            // if running and started...
            if (_run && _isStarted)
            {
                // wait until the lock is released..
                lock (_pulse)
                    Monitor.Wait(_pulse);
            }
        }

        /// <summary>
        /// the task given to the monitor thread.
        /// </summary>
        private void MonitorTask()
        {
            // loop while the run flag is true.
            while (_run)
            {
                try
                {
                    // has the last status been taken?
                    if (_last == null)
                    {
                        // snapshot the current status.
                        _last = new NetworkStatus();

                        // sleep for the duration of the poll interval.
                        Thread.Sleep(_waitInterval);

                        // run to the next iteration.
                        continue;
                    }
                    // get the current network status:
                    var current = new NetworkStatus();

                    // test for changes and raise events where neccessary.
                    if (NetworkInterfaceConnected != null && _monitorNewConnections)
                    {
                        // evaluate all the network interfaces that have connected since the
                        // last snapshot
                        foreach (NetworkInterface ni in current.Connected(_last))
                        {
                            // test if the network interface was in the last snapshot:
                            OperationalStatus lastStatus = OperationalStatus.NotPresent;
                            if (_last.Contains(ni.Id))
                                lastStatus = _last[ni.Id].OperationalStatus;

                            // raise the interface connected event:
                            NetworkInterfaceConnected(this, new StatusMonitorEventArgs
                                                                {
                                                                    EventType = StatusMonitorEventType.Connected,
                                                                    Interface = ni,
                                                                    LastOperationalStatus = lastStatus
                                                                });
                        }
                    }

                    // test for interface dis-connections
                    if (NetworkInterfaceDisconnected != null && _monitorDisconnections)
                    {
                        // enumerate the network interfaces that were Up but are not now.
                        foreach (NetworkInterface ni in current.Disconnected(_last))
                        {
                            // raise the interface dis-connected event:
                            NetworkInterfaceDisconnected(this, new StatusMonitorEventArgs
                                                                   {
                                                                       // set the event-type, interface and last status.
                                                                       EventType =
                                                                           StatusMonitorEventType.Disconnected,
                                                                       Interface = ni,
                                                                       LastOperationalStatus = OperationalStatus.Up
                                                                   });
                        }
                    }

                    // test for interface changes.
                    if (NetworkInterfaceChanged != null && _monitorAnyStatusChange)
                    {
                        // enumerate the interfaces that have changed status in any way since 
                        // the last snapshot.
                        foreach (NetworkInterface ni in current.Changed(_last))
                        {
                            // find the last status of the interface:
                            OperationalStatus lastStatus = OperationalStatus.NotPresent;
                            if (_last.Contains(ni.Id))
                                lastStatus = _last[ni.Id].OperationalStatus;

                            // raise the interface changed event:
                            NetworkInterfaceChanged(this, new StatusMonitorEventArgs
                                                              {
                                                                  // set the event-type interface and last status.
                                                                  EventType = StatusMonitorEventType.Changed,
                                                                  Interface = ni,
                                                                  LastOperationalStatus = lastStatus
                                                              });
                        }
                    }

                    if (NetworkAddressChanged != null && _monitorNetworkAddressChanged)
                    {
                        // enumerate the interfaces that have changed status in any way since 
                        // the last snapshot.
                        foreach (NetworkInterface ni in current.AddressChange(_last))
                        {
                            // find the last status of the interface:
                            OperationalStatus lastStatus = OperationalStatus.NotPresent;
                            if (_last.Contains(ni.Id))
                                lastStatus = _last[ni.Id].OperationalStatus;

                            // raise the interface changed event:
                            NetworkAddressChanged(this, new StatusMonitorEventArgs
                                                            {
                                                                // set the event-type interface and last status.
                                                                EventType = StatusMonitorEventType.AddressChanged,
                                                                Interface = ni,
                                                                LastOperationalStatus = lastStatus
                                                            });
                        }
                    }

                    // set last to the current.
                    _last = current;

                    // wait...
                    if (_run)
                        Thread.Sleep(_waitInterval);

                    // pulse any threads waiting in WaitForPoll.
                    lock (_pulse)
                        Monitor.PulseAll(_pulse);
                }
                catch (Exception exception)
                {
                    // handle the exception....(real exception handler should go here)
                    Console.WriteLine(exception.ToString());

                    //// increment the exception counter.
                    //Interlocked.Increment(ref _exceptionCount);
                }
            }
        }

        #endregion

        #region Types

        /// <summary>
        /// enumeration of types of interface monitor event.
        /// </summary>
        public enum StatusMonitorEventType
        {
            /// <summary>
            /// network interface has connected.
            /// </summary>
            Connected,

            /// <summary>
            /// network interface has disconnected.
            /// </summary>
            Disconnected,

            /// <summary>
            /// network interface status has changed.
            /// </summary>
            Changed,

            /// <summary>
            /// network Address has changed.
            /// </summary>
            AddressChanged
        }

        /// <summary>
        /// event arguments for an interface monitor event.
        /// </summary>
        public class StatusMonitorEventArgs : EventArgs
        {
            /// <summary>
            /// the affected interface.
            /// </summary>
            public NetworkInterface Interface { get; set; }

            /// <summary>
            /// the operational status of the interface at the last poll.
            /// </summary>
            public OperationalStatus LastOperationalStatus { get; set; }

            /// <summary>
            /// the type of the event (ie, connection, disconnection, change)
            /// </summary>
            public StatusMonitorEventType EventType { get; set; }
        }

        #endregion
    }
}