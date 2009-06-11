using Commerce.Data;
using System.Collections.Generic;

namespace Commerce.Services {
    public interface IPersonalizationService {
        
        UserProfile GetProfile(string userName);
        void SaveUserEvent(UserEvent userEvent);
        void SaveCategoryView(string userName, string IP, Category category);
        void SaveProductView(string userName, string IP, Product product);

    }
}
