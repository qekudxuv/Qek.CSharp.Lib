using System;

namespace Qek.NHibernate.Model
{
    /// <summary>
    /// DateTime:   2013/07/31
    /// Author:     Sam.SH_Chang#21978
    /// each entities are supposed to inherit this class.
    /// private field [_id] presents TABLE's PK
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    [Serializable]
    public abstract class DomainObject<T>
    {
        private T _id = default(T);

        /// <summary>
        /// ID may be of type string, int, custom type, etc.
        /// Setter is protected to allow unit tests to set this property via reflection and to allow 
        /// domain objects more flexibility in setting this for those objects with assigned IDs.
        /// </summary>
        public virtual T ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public override bool Equals(object obj)
        {
            DomainObject<T> compareTo = obj as DomainObject<T>;

            return (compareTo != null) &&
                   (
                        HasSameNonDefaultIdAs(compareTo) ||
                        // Since the IDs aren't the same, either of them must be transient to 
                        // compare business value signatures
                        (
                            (IsTransient() || compareTo.IsTransient()) &&
                            HasSameBusinessSignatureAs(compareTo)
                        )
                    );
        }

        /// <summary>
        /// Transient objects are not associated with an item already in storage.  For instance,
        /// a <see cref="Customer" /> is transient if its ID is 0.
        /// </summary>
        public virtual bool IsTransient()
        {
            return ID == null || ID.Equals(default(T));
        }

        /// <summary>
        /// Must be provided to properly compare two objects
        /// </summary>
        public abstract override int GetHashCode();

        private bool HasSameBusinessSignatureAs(DomainObject<T> compareTo)
        {
            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Returns true if self and the provided persistent object have the same ID values 
        /// and the IDs are not of the default ID value
        /// </summary>
        private bool HasSameNonDefaultIdAs(DomainObject<T> compareTo)
        {
            return (ID != null && !ID.Equals(default(T))) &&
                   (compareTo.ID != null && !compareTo.ID.Equals(default(T))) &&
                   ID.Equals(compareTo.ID);
        }
    }
}

