using NModbus;
using System.Net.Sockets;

namespace WpfApp4
{
    public class ModbusMaster
    {
        public int ModbusTcpMasterReadHoldingRegistr(ushort regisrt, string ip)
        {
            using (TcpClient tcpClient = new TcpClient(ip, 502))
            {
                var factory = new ModbusFactory();
                IModbusMaster modbusMaster = factory.CreateMaster(tcpClient);
                var result = modbusMaster.ReadHoldingRegisters(1, regisrt, 1);

                return result[0];

            }
        }
    }
}
