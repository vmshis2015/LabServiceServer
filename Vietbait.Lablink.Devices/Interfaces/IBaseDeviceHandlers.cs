namespace Vietbait.Lablink.Devices
{
    internal interface IBaseDeviceHandlers
    {
        void OnEndReceiveData(object obj);
        void OnBeginReceiveData(object obj);
        void OnInCommingData(object obj);
    }
}