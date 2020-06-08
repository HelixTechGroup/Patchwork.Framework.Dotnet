namespace Patchwork.Framework.Platform.Interop.User32 {
    public enum SystemParametersTimeoutInfo
    {
        /// <summary>
        ///     Retrieves the number of milliseconds that a thread can go without dispatching a message before the system considers
        ///     it unresponsive. The pvParam parameter must point to an integer variable that receives the value.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETHUNGAPPTIMEOUT = 0x0078,

        /// <summary>
        ///     Retrieves the number of milliseconds that the system waits before terminating an application that does not respond
        ///     to a shutdown request. The pvParam parameter must point to an integer variable that receives the value.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETWAITTOKILLTIMEOUT = 0x007A,

        /// <summary>
        ///     Retrieves the number of milliseconds that the service control manager waits before terminating a service that does
        ///     not respond to a shutdown request. The pvParam parameter must point to an integer variable that receives the value.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_GETWAITTOKILLSERVICETIMEOUT = 0x007C,

        /// <summary>
        ///     Sets the hung application time-out to the value of the uiParam parameter. This value is the number of milliseconds
        ///     that a thread can go without dispatching a message before the system considers it unresponsive.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETHUNGAPPTIMEOUT = 0x0079,

        /// <summary>
        ///     Sets the application shutdown request time-out to the value of the uiParam parameter. This value is the number of
        ///     milliseconds that the system waits before terminating an application that does not respond to a shutdown request.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETWAITTOKILLTIMEOUT = 0x007B,

        /// <summary>
        ///     Sets the service shutdown request time-out to the value of the uiParam parameter. This value is the number of
        ///     milliseconds that the system waits before terminating a service that does not respond to a shutdown request.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP/2000:  This parameter is not supported.
        /// </summary>
        SPI_SETWAITTOKILLSERVICETIMEOUT = 0x007D
    }
}