using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserDataBase.EFcoreModels.CRUD
{
    public class CrudService
    {
        UserDb _db;
        UserFilesDb _f;
        public CrudService(UserDb db, UserFilesDb files)
        {
            _db = db;
            _f = files;
        }
        public async Task AddComment(Comment comment)
        {
            await _db.AddAsync(comment);
            await _db.SaveChangesAsync();
        }
        
    }
}
