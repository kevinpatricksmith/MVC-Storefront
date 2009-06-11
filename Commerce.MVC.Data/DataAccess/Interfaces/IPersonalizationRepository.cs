using Commerce.Data;
using System.Linq;

namespace Commerce.Data {
    public interface IPersonalizationRepository {

        IQueryable<UserEvent> GetEvents();
        void Save(UserEvent userEvent);
        void MigrateProfile(string fromUserName, string toUserName);
    }
}
