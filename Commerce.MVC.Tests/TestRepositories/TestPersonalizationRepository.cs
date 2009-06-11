
using Commerce.Data;
using System.Linq;
using System.Collections.Generic;

namespace Commerce.Tests {

    public class TestPersonalizationRepository : IPersonalizationRepository {

        IList<UserEvent> eventList;

        public TestPersonalizationRepository()
        {

            eventList = new List<UserEvent>();
            ICatalogRepository catalogRepository = new TestCatalogRepository();
            
            //looking at products
            for (int i = 1; i < 50; i++)
            {

                for (int x = 10; x <= 20; x++)
                {
                    Category c = catalogRepository
                        .GetCategories().WithCategoryID(x).Take(1).SingleOrDefault();
                    
                    //take the first
                    Product p = c.Products[0];

                    UserEvent ue = new UserEvent("testuser", "127.0.0.1", c.ID, p.ID, System.Guid.Empty, 
                        UserBehavior.ViewProduct);
                    
                    eventList.Add(ue);

                    //skew twice for 12
                    c = catalogRepository
                        .GetCategories().WithCategoryID(12).Take(1).SingleOrDefault();

                    p = c.Products[0];

                    ue = new UserEvent("testuser", "127.0.0.1", c.ID, p.ID, System.Guid.Empty,
                        UserBehavior.ViewProduct);
                    
                    eventList.Add(ue);
                }


            }

            //view some products
            for (int i = 1; i <= 5; i++)
            {

                Product p = catalogRepository.GetProducts().WithProductID(i).SingleOrDefault();
                UserEvent ue = new UserEvent("testuser", "127.0.0.1", null, p.ID, System.Guid.Empty,
                    UserBehavior.ViewProduct);
                
                eventList.Add(ue);

            }

        }


        public IQueryable<UserEvent> GetEvents()
        {
            return eventList.AsQueryable();
        }



        public void Save(UserEvent userEvent)
        {
            eventList.Add(userEvent);
        }


        public void MigrateProfile(string fromUserName, string toUserName) {
            //throw new System.NotImplementedException();
        }

    }
}
