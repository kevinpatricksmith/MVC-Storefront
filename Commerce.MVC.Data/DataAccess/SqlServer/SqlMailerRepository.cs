using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Data.SqlRepository {
    public class SqlMailerRepository:IMailerRepository {

        DB _db;
        public SqlMailerRepository() {
            _db = new DB();
        }
        public SqlMailerRepository(DB db) {
            _db=db;
        }

        public IQueryable<Mailer> GetMailerTemplates(string languageCode) {

            //this can happen if passed a null DB
            return from m in _db.MailerTemplates
                   join mt in _db.MailerTemplateTypes on m.MailerTypeID equals mt.MailerTypeID
                   where m.Culture.LanguageCode == languageCode
                   select new Mailer
                   {
                       ID=m.MailerTypeID,
                       Body=m.Body,
                       Subject=m.Subject,
                       IsHtml=m.IsHtml,
                       MailType=(MailerType)m.MailerTypeID,
                       
                   };



        }

    }
}
