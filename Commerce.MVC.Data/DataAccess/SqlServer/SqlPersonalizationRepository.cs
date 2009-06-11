using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data.SqlRepository;
using Commerce.Data;

namespace Commerce.Services {
    public class SqlPersonalizationRepository : IPersonalizationRepository {
        
        DB _db;

        public SqlPersonalizationRepository(DB db) {
            _db = db;
        }



        public IQueryable<Commerce.Data.UserEvent> GetEvents()
        {
            return from ue in _db.UserEvents
                   select new Commerce.Data.UserEvent
                   {
                       UserName = ue.UserName,
                       IP = ue.IP,
                       Behavior = (UserBehavior)ue.UserBehaviorID,
                       CategoryID = ue.CategoryID,
                       ProductID = ue.ProductID,
                       DateCreated = ue.EventDate,
                       OrderID = ue.OrderID
                   };
        }


        public void Save(Commerce.Data.UserEvent userEvent)
        {


            using (DB db = new DB())
            {
                //make sure there's a user
                int userCount = (from u in db.Users
                                 where u.UserName == userEvent.UserName
                                 select u).Count();

                //if not, need to add one
                if (userCount == 0)
                {
                    Commerce.Data.SqlRepository.User newUser = new Commerce.Data.SqlRepository.User();
                    newUser.UserName = userEvent.UserName;
                    newUser.CreatedOn = DateTime.Now;
                    newUser.ModifiedOn = DateTime.Now;
                    db.Users.InsertOnSubmit(newUser);
                }

                //there is no updating of user events - it's always an insert
                Commerce.Data.SqlRepository.UserEvent newEvent = new Commerce.Data.SqlRepository.UserEvent();

                //some left/right
                newEvent.IP = userEvent.IP;
                newEvent.UserName = userEvent.UserName;
                newEvent.ProductID = userEvent.ProductID;
                newEvent.CategoryID = userEvent.CategoryID;
                newEvent.EventDate = DateTime.Now;
                if (userEvent.OrderID != Guid.Empty)
                    newEvent.OrderID = userEvent.OrderID;
                else
                    newEvent.OrderID = null;
                newEvent.UserBehaviorID = (int)userEvent.Behavior;

                db.UserEvents.InsertOnSubmit(newEvent);
                db.SubmitChanges();
            }
        }


        public void MigrateProfile(string fromUserName, string toUserName) {

            //find the old userevents
            //and update them to the toUserName
            var events = GetEvents().Where(x => x.UserName == fromUserName);
            foreach (var item in events) {
                item.UserName = toUserName;
            }
            _db.SubmitChanges();

        }

    }
}
