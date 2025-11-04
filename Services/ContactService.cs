using Google.Cloud.Firestore;
using UmbiloTemple.Models;

namespace UmbiloTemple.Services
{
    public class ContactService
    {
        private readonly FirestoreDb _db;
        public ContactService(FirestoreDb db) => _db = db;

        public async Task SaveMessageAsync(ContactMessage msg)
        {
            var docRef = _db.Collection("ContactMessages").Document(msg.Id);
            await docRef.SetAsync(msg);
        }
    }
}
