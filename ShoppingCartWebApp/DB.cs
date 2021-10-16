using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ShoppingCartWebApp.Models;

namespace ShoppingCartWebApp
{
    public class DB
    {
        private DBContext dbContext;

        public DB(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public void Seed()
        {
            SeedUsersTable();
            SeedProductsTable();
        }

        // Data Function List, choose function to use in your controller



        /*Login related methods
         1) checkUser whether exist
            method: checkUserWhetherExist()   parameters: string username

         2) check username and password both whether correct
            method: checkUserNameAndPassword(string username, string password)
                    parameters: string username , string password

         3) insert/or delete data into seesion table, return true(success) or false(failed)
            method: SeedSessionData(Session session)  parameters: Session object
            method: DeleteSessionData(Session session)  parameters: Session object
         */




        /* Libraries Gallery related methods
         4) List of Libraries
            method:  GetProductsList()    
            return data is a List of Product Object

         5) method: SearchProducts(string searchStr)


         6) method: AddLibraryToCart(Guid userId,Guid ProductId)


         7) method: getCartViewList(Guid userId)   return a list of cart object
        */

        //1) checkUser whether exist
        public bool checkUserWhetherExist(string username) {

            User user = dbContext.Users.FirstOrDefault(
                x => x.Username == username
                ) ;

            if (user == null)
            {
                Debug.WriteLine("this user not registered");
                return false;
            }
            else {

                return true;
            }

        }

        //2) check username and password both whether correct
        public bool checkUserNameAndPassword(string username, string password) {

            User user = dbContext.Users.FirstOrDefault(
                x => x.Username == username
                );

            if (user != null)
            {
                HashAlgorithm sha = SHA256.Create();
                byte[] Passhash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

                if (Passhash == user.PassHash)
                {
                    //username and password both correct, can success login
                    return true;
                }
                else {
                    Debug.WriteLine("Password not success");
                    return false;
                }


            }
            else {
                Debug.WriteLine("this user not registered");
                return false;
            }

        }




        // 3) insert Session to 
        public bool SeedSessionData(Session session) {

            Session session1 = dbContext.Sessions.FirstOrDefault(
                x => x.Id == session.Id
                );

            if (session1 == null)
            {
                dbContext.Sessions.Add(session);
                dbContext.SaveChanges();
                return true;
            }
            else {

                Debug.WriteLine("this Session already exist");
                return false;
            }

        }


        public bool DeleteSessionData(Session session)
        {
            Session session1 = dbContext.Sessions.FirstOrDefault(
                x => x.Id == session.Id
                );
            if (session1 == null)
            {
                Debug.WriteLine("no this Session");
                return false;
            }
            else
            {
                dbContext.Sessions.Remove(session);
                dbContext.SaveChanges();
                return true;
            }
        }

        //4)
        public List<Product> GetProductsList() {

            List<Product> products = dbContext.products.ToList();
            if (products != null)
            {
                return products;
            }

            return new List<Product>();
        }


        //5)
        public List<Product> SearchProducts(string searchStr) {

            if (searchStr == null)
            {
                searchStr = "";
            }
            List<Product> products = dbContext.products.Where(
                x => x.ProductName.Contains(searchStr)
                || x.Description.Contains(searchStr)
                ).ToList();

            if (products != null)
            {
                return products;
            }
            else {

                return new List<Product>();
            
            }

        }

        public void AddLibraryToCart(Guid userId,Guid ProductId) {

            User user = dbContext.Users.FirstOrDefault(
                x => x.Id == userId
                );

            Product productData = dbContext.products.FirstOrDefault(
                x => x.Id == ProductId
                );

            if (user == null || productData == null)
            {
                Debug.WriteLine("user or product not exist");
                return;
            }

            Cart cart = new Cart
            {
            };

            productData.carts.Add(cart);
            user.carts.Add(cart);

            dbContext.SaveChanges();

        }


        //get getCartViewList
        public void getCartViewList(Guid userId) {

            List<List<Cart>> carts = (List<List<Cart>>)dbContext.carts.Where(
                x => x.user.Id == userId
                ).GroupBy(
                x => x.product
                );

            foreach (var item in carts)
            {

            }

            if (carts == null)
            {
                //return carts;
            }
            else {
               // return new List<Cart>();
            }

        }


        //seedUsers
        public void SeedUsersTable()
        {
            HashAlgorithm sha = SHA256.Create();

            string[] usernames = { "john", "jean", "james", "kate","david","crist"};


            foreach (string username in usernames)
            { 
                string combo = username + username;
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combo));

                dbContext.Add(new User
                {
                    Username = username,
                    PassHash = hash
                });
            }

            dbContext.SaveChanges();
        }



        //seedProducts
        public void SeedProductsTable() {

            dbContext.Add(new Product
            {
                ProductName = ".NET Charts",
                Description = "Brings powerful charting capabilities to your .NET applications.",
                Price = 99,
                imageUrl = "../Image/charts.png"
            }) ;


            dbContext.Add(new Product
            {
                ProductName = ".NET PayPal",
                Description = "Integrate your .NET apps with Paypal the easy way!",
                Price = 69,
                imageUrl = "../Image/PayPal.png"
            });


            dbContext.Add(new Product
            {
                ProductName = ".NET ML",
                Description = "Supercharged .NET machine learning libraries",
                Price = 299,
                imageUrl = "../Image/ML.png"
            });


            dbContext.Add(new Product
            {
                ProductName = ".NET Analytics",
                Description = "Performs data mining and analytics easily in .NET",
                Price = 299,
                imageUrl = "../Image/Analytics.png"
            });

            dbContext.Add(new Product
            {
                ProductName = ".NET Logger",
                Description = "Logs and aggregates events easily in your .NET apps",
                Price = 49,
                imageUrl = "../Image/Logger.png"
            });


            dbContext.Add(new Product
            {
                ProductName = ".NET Numerics",
                Description = "Powerful numerical methods for your .NET simulations",
                Price = 99,
                imageUrl = "../Image/Numerics.png"
            });

            dbContext.SaveChanges();


        }


    }
}
