using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;
using System.ComponentModel;

namespace Repository.Pattern.Ef6
{
    public abstract class Entity : IObjectState
    {
        private bool _isActive = true;
        [NotMapped]
        public ObjectState ObjectState { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    }
}