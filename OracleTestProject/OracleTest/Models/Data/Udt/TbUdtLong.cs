using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace OracleTest.Models.Data.Udt
{
    public class TbUdtLong : INullable, IOracleCustomType
    {
        private bool _isNull;
        private IEnumerable<UdtLong> _udtLongData;

        public virtual bool IsNull
        {
            get
            {
                return _isNull;
            }
        }

        public static TbUdtLong Null
        {
            get
            {
                TbUdtLong udt = new TbUdtLong();
                udt._isNull = true;
                return udt;
            }
        }

        [OracleObjectMapping("UDTLONGDATA")]
        public IEnumerable<UdtLong> UdtLongData
        {
            get
            {
                return _udtLongData;
            }
            set
            {
                _udtLongData = value;
            }
        }

        public void FromCustomObject(OracleConnection con, object udt)
        {
            if (_udtLongData != null)
            {
                OracleUdt.SetValue(con, udt, "UDTLONGDATA", _udtLongData);
            }
        }

        public void ToCustomObject(OracleConnection con, object udt)
        {
            _udtLongData = (IEnumerable<UdtLong>)OracleUdt.GetValue(con, udt, "UDTLONGDATA");
        }
    }

    [OracleCustomTypeMapping("TB_UDT_LONG")]
    public class TbUdtLongFactory : IOracleCustomTypeFactory
    {
        // Implementation of IOracleCustomTypeFactory.CreateObject()
        public IOracleCustomType CreateObject()
        {
            // Return a new custom object
            return new TbUdtLong();
        }
    }
}
