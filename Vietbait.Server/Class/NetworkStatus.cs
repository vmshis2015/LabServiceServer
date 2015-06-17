/*******************************************************************************
 *******************************************************************************
            Author: Simon Bridge, June 2011 mailto:srbridge@gmail.com
 
 
        This code is provided under the Code Project Open Licence (CPOL)
          See http://www.codeproject.com/info/cpol10.aspx for details
  
 *******************************************************************************
 ******************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Vietbait.NetworkMonitor
{
    /// <summary>
    /// keeps a record of the NetworkInterfaces on the current machine at the time of construction and
    /// offers methods to compare it with other NetworkStatus objects.
    /// Implements the IEnumerable interface.
    /// </summary>
    public class NetworkStatus : IEnumerable<NetworkInterface>
    {
        #region IEnumerable<NetworkInterface> Members

        /// <summary>
        /// return an enumerator for the network interface collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<NetworkInterface> GetEnumerator()
        {
            foreach (var pair in _status)
                yield return pair.Value.Interface;
        }

        /// <summary>
        /// return an enumerator for the network interface collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var pair in _status)
                yield return pair.Value;
        }

        #endregion

        #region Nested type: NiStatusRecord

        /// <summary>
        /// keeps a record of a network interface status.
        /// </summary>
        public class NiStatusRecord
        {
            /// <summary>
            /// construct the network interface status record.
            /// </summary>
            /// <param name="ni"></param>
            public NiStatusRecord(NetworkInterface ni)
            {
                Interface = ni;
                OperationalStatus = ni.OperationalStatus;
                Type = ni.NetworkInterfaceType;
                Speed = ni.Speed;
                Name = ni.Name;
            }

            /// <summary>
            /// the network interface
            /// </summary>
            public NetworkInterface Interface { get; set; }

            /// <summary>
            /// the recorded operational status of the network interface at the time this class was constructed.
            /// </summary>
            public OperationalStatus OperationalStatus { get; set; }

            /// <summary>
            /// the recorded type of the network interface at the tine this class was constructed.
            /// </summary>
            public NetworkInterfaceType Type { get; set; }

            /// <summary>
            /// the recorded speed of the network interface at the time this class was constructed.
            /// </summary>
            public long Speed { get; set; }

            /// <summary>
            /// net work interface name.
            /// </summary>
            public string Name{ get; set; }

            /// <summary>
            /// network Address.
            /// </summary>
            public string IPAddress { get; set; }
            
        }

        #endregion

        #region Fields

        /// <summary>
        /// a dictionary containing all the network interface status records (one for each NI on the host machine)
        /// </summary>
        private readonly Dictionary<String, NiStatusRecord> _status = new Dictionary<string, NiStatusRecord>();

        #endregion

        #region Constructor

        /// <summary>
        /// construct the network status object and snapshot the current status.
        /// </summary>
        public NetworkStatus()
        {
            // snapshot the network status:
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                _status.Add(ni.Id, new NiStatusRecord(ni));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// the default property: access the network interface by it's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NetworkInterface this[String id]
        {
            get { return _status[id].Interface; }
            set { _status[id].Interface = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// determine if the network interface with the given id exists in the collection.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(String id)
        {
            return _status.ContainsKey(id);
        }

        #endregion

        #region Queries

        /// <summary>
        /// list of network interfaces that have become slower.
        /// </summary>
        /// <param name="lastStatus"></param>
        /// <returns></returns>
        public IEnumerable<NetworkInterface> Slower(NetworkStatus lastStatus)
        {
            foreach (var status in _status)
            {
                if (lastStatus._status.ContainsKey(status.Key))
                {
                    // if the last recorded interface speed was greater than the current speed,
                    // this interface is now slower.
                    if (lastStatus._status[status.Key].Speed > status.Value.Speed)
                        yield return status.Value.Interface;
                }
            }
        }

        /// <summary>
        /// list of network interfaces that have become faster.
        /// </summary>
        /// <param name="lastStatus"></param>
        /// <returns></returns>
        public IEnumerable<NetworkInterface> Faster(NetworkStatus lastStatus)
        {
            foreach (var status in _status)
            {
                if (lastStatus._status.ContainsKey(status.Key))
                {
                    // if the last recorded interface speed was less than the current speeed, then
                    // this interface is now faster.
                    if (lastStatus._status[status.Key].Speed < status.Value.Speed)
                        yield return status.Value.Interface;
                }
            }
        }

        /// <summary>
        /// get an enumerable of network interfaces that have changed since the last status
        /// snapshot.
        /// </summary>
        /// <param name="lastStatus"></param>
        /// <returns></returns>
        public IEnumerable<NetworkInterface> Changed(NetworkStatus lastStatus)
        {
            // enumerate the current snapshot values.
            foreach (var pair in _status)
            {
                // check this network interface was in the last snapshot:
                if (lastStatus._status.ContainsKey(pair.Key))
                {
                    // check if the status changed...
                    if (lastStatus._status[pair.Key].OperationalStatus != pair.Value.OperationalStatus)
                    {
                        // this network interface has changed since the last snapshot:
                        yield return pair.Value.Interface;
                    }
                }
                else
                    // this network interface is new..
                    yield return pair.Value.Interface;
            }

            // enumerate the last snapshot values:
            foreach (var pair in lastStatus._status)
            {
                // if this network interface was not in the last test results, it has changed.
                if (!_status.ContainsKey(pair.Key))
                {
                    // this network interface has disapeared since the last snapshot
                    yield return pair.Value.Interface;
                }
            }
        }

        /// <summary>
        /// get an enumerable of network interfaces that have connected since the last status. this includes "UP" adapters that
        /// were not present in the last test.
        /// </summary>
        /// <param name="lastStatus">the last NetworkStatus test results.</param>
        /// <returns>
        /// an enumerable of the newly connected NetworkInterface objects
        /// </returns>
        public IEnumerable<NetworkInterface> Connected(NetworkStatus lastStatus)
        {
            // enumerate the current list of interfaces:
            foreach (var pair in _status)
            {
                // determine if the interface was in the last snapshot:
                if (lastStatus._status.ContainsKey(pair.Key))
                {
                    //if (pair.Value.Interface.Description.ToUpper().StartsWith("MICROSOFT")) continue;
                    // if the current status is "Up" and the last  status wasn't, the interface has connected.
                    if (lastStatus._status[pair.Key].OperationalStatus != OperationalStatus.Up &&
                        pair.Value.OperationalStatus == OperationalStatus.Up)
                        yield return pair.Value.Interface;
                }
                else
                {
                    // if the interface was not in the last snapshot, and is "up" then it has connected.
                    if (pair.Value.OperationalStatus == OperationalStatus.Up)
                        yield return pair.Value.Interface;
                }
            }
        }

        /// <summary>
        /// get an enumerable of network interfaces that have disconnected since the last status.
        /// </summary>
        /// <param name="lastStatus">last network status snapshot</param>
        /// <returns></returns>
        public IEnumerable<NetworkInterface> Disconnected(NetworkStatus lastStatus)
        {
            // enumerate the current list:
            foreach (var pair in _status)
                // was this interface in the last snapshot?
                if (lastStatus._status.ContainsKey(pair.Key))
                {
                    //if (pair.Value.Interface.Description.ToUpper().StartsWith("MICROSOFT")) continue;
                    // if it was Up and isn't up now, it has disconnected....
                    if (lastStatus._status[pair.Key].OperationalStatus == OperationalStatus.Up &&
                        pair.Value.OperationalStatus != OperationalStatus.Up)
                        yield return pair.Value.Interface;
                }
                   
            // enumerate the last list:
            foreach (var pair in lastStatus._status)
                // is this network interface in the current list?
                if (!_status.ContainsKey(pair.Key))
                    // this network interface has disappeared....if it was UP on the last status, it has disconnected....
                    if (pair.Value.OperationalStatus == OperationalStatus.Up)
                        yield return pair.Value.Interface;
        }


        /// <summary>
        /// get an enumerable of network interfaces that have disconnected since the last status.
        /// </summary>
        /// <param name="lastStatus">last network status snapshot</param>
        /// <returns></returns>
        public IEnumerable<NetworkInterface> AddressChange(NetworkStatus lastStatus)
        {
            // enumerate the current snapshot values.
            var result = new List<NetworkInterface>();
           
                foreach (var pair in _status)
                {
                    // check this network interface was in the last snapshot:
                    if (lastStatus._status.ContainsKey(pair.Key))
                    {
                        //if (pair.Value.Interface.Description.ToUpper().StartsWith("MICROSOFT")) continue;
                        // check if the status changed...
                        var lastAddress = lastStatus._status[pair.Key].Interface.GetIPProperties().UnicastAddresses;
                        var address = pair.Value.Interface.GetIPProperties().UnicastAddresses;
                        //lastAddress[0].Address.ToString();

                        try
                        {
                            if (lastAddress[0].Address.ToString() != address[0].Address.ToString())
                                // this network interface has changed since the last snapshot:
                                result.Add(pair.Value.Interface);
                        }
                        catch (Exception exception)
                        {
                          continue;
                        }
                        try
                        {
                            if (lastAddress[1].Address.ToString() != address[1].Address.ToString())
                                // this network interface has changed since the last snapshot:
                                result.Add(pair.Value.Interface);
                        }
                        catch (Exception exception)
                        {
                            continue;
                        }
                    }
                    else
                        // this network interface is new..
                        result.Add(pair.Value.Interface);
                }
                return result;
        }

        #endregion
    }
}