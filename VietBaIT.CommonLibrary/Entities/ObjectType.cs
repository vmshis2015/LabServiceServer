using System;
using SubSonic;
using VietBaIT.HISLink.DataAccessLayer;

namespace VietBaIT.CommonLibrary.Entities
{
    [Serializable]
    public class ObjectType
    {
        #region Private Variables

        private LObjectType objectType;
        #endregion

        #region Constructor
        public ObjectType() {}

        private ObjectType(LObjectType objectType)
        {
            this.objectType = objectType;
        }
        #endregion

        #region Public Properties
        public short ObjectTypID
        {
            get { return objectType.ObjectTypeId; }
            set { objectType.ObjectTypeId = value; }
        }

        public string ObjectTypeName
        {
            get { return objectType.ObjectTypeName; }
            set { objectType.ObjectTypeName = value; }
        }

        public short Order
        {
            get { return objectType.IntOrder; }
            set { objectType.IntOrder = value; }
        }

      

        public string Description
        {
            get { return objectType.SDesc; }
            set { objectType.SDesc = value; }
        }
       
        #endregion

        #region Public Methods
        public ObjectType GetByID(int objectTypeID)
        {
            ObjectType result = null;
            var findobjectType = ReadOnlyRecord<LObjectType>.FetchByID(objectTypeID); 
            if (findobjectType != null)
                result = new ObjectType(findobjectType);

            return result;
        }
        #endregion
    }
}