using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace OracleTest.Models.Data.Udt
{
    public class UdtLong : INullable, IOracleCustomType
    {
        private bool _isNull;
        private long? _longData;

        public virtual bool IsNull
        {
            get
            {
                return _isNull;
            }
        }

        public static UdtLong Null
        {
            get
            {
                UdtLong udt = new UdtLong();
                udt._isNull = true;
                return udt;
            }
        }

        [OracleObjectMapping("LONGDATA")]
        public long? LongData
        {
            get
            {
                return _longData;
            }
            set
            {
                _longData = value;
            }
        }

        public virtual void FromCustomObject(OracleConnection con, object udt)
        {
            if (_longData != null)
            {
                OracleUdt.SetValue(con, udt, "LONGDATA", _longData);
            }
        }

        public virtual void ToCustomObject(OracleConnection con, object udt)
        {
            _longData = (long?)OracleUdt.GetValue(con, udt, "LONGDATA");
        }
    }

    [OracleCustomTypeMapping("UDT_LONG")]
    public class UdtLongFactory : IOracleCustomTypeFactory
    {
        // Implementation of IOracleCustomTypeFactory.CreateObject()
        public IOracleCustomType CreateObject()
        {
            // Return a new custom object
            return new UdtLong();
        }
    }
}
