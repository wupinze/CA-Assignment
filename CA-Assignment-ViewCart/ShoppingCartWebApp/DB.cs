﻿using System;
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


         7) method: getCartViewList(Guid userId)   return two list of cart object and quantity

         8) method: ReduceProductFromCart(Guid userId,Guid ProductId)
        
         9) method: checkOutCartView(Guid userId)

         10)method: getPurchaseHistory(Guid userId)
         */

        //1) checkUser whether exist
        public bool checkUserWhetherExist(string username)
        {

            User user = dbContext.Users.FirstOrDefault(
                x => x.Username == username
                );

            if (user == null)
            {
                Debug.WriteLine("this user not registered");
                return false;
            }
            else
            {

                return true;
            }

        }

        //2) check username and password both whether correct
        public bool checkUserNameAndPassword(string username, string password)
        {

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
                else
                {
                    Debug.WriteLine("Password not success");
                    return false;
                }


            }
            else
            {
                Debug.WriteLine("this user not registered");
                return false;
            }

        }




        // 3) insert Session to 
        public bool SeedSessionData(Session session)
        {

            Session session1 = dbContext.Sessions.FirstOrDefault(
                x => x.Id == session.Id
                );

            if (session1 == null)
            {
                dbContext.Sessions.Add(session);
                dbContext.SaveChanges();
                return true;
            }
            else
            {

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
        public List<Product> GetProductsList()
        {

            List<Product> products = dbContext.products.ToList();
            if (products != null)
            {
                return products;
            }

            return new List<Product>();
        }


        //5)
        public List<Product> SearchProducts(string searchStr)
        {
            List<Product> products;
            if (string.IsNullOrEmpty(searchStr))
                products = dbContext.products.ToList();
            else
            {
                products = dbContext.products.Where(
                    x => x.ProductName.Contains(searchStr)
                    || x.Description.Contains(searchStr)
                    ).ToList();
            }

            foreach (var product in products)
                Debug.WriteLine(product.ProductName);

            if (products != null)
            {
                return products;
            }
            else
            {

                return new List<Product>();

            }

        }

        //6)
        public void AddLibraryToCart(Guid userId, Guid ProductId)
        {

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


        //7)get getCartViewList
        /*  deal with back Data
           
         var tupList = db.getCartViewList(user.Id);  // call method

                List<int> QuantityList = tupList.Item1;
                List<Product> ProductList = tupList.Item2;
         */

        public Tuple<List<int>, List<Product>> getCartViewList(string username)
        {

            List<Cart> carts = dbContext.carts.Where(
                x => x.user.Username == username
                ).ToList();

            if (carts != null)
            {
                var iter = from cart in carts
                           group cart by cart.product into productGroup
                           select productGroup;


                List<int> QuantityList = new List<int>();
                List<Product> ProductList = new List<Product>();

                foreach (var grp in iter)
                {
                    //Console.WriteLine("{0}", grp.Count());
                    QuantityList.Add(grp.Count());
                    foreach (var cart in grp)
                    {
                        if (!ProductList.Contains(cart.product))
                        {
                            ProductList.Add(cart.product);
                        }
                        //Console.WriteLine("{0}", cart.product.ProductName);
                    }
                }

                return new Tuple<List<int>, List<Product>>(QuantityList, ProductList);

            }
            else
            {

                return new Tuple<List<int>, List<Product>>(new List<int>(), new List<Product>());
            }



        }


        //8)
        public void ReduceProductFromCart(string username, Guid ProductId)
        {

            User user = dbContext.Users.FirstOrDefault(
                x => x.Username == username
                );

            Product productData = dbContext.products.FirstOrDefault(
                x => x.Id == ProductId
                );

            if (user == null || productData == null)
            {
                Debug.WriteLine("userId or productId not correct");
                return;
            }
            Cart cart = dbContext.carts.FirstOrDefault(
                x => x.user == user && x.product == productData
                );
            if (cart != null)
            {
                //productData.carts.Remove(cart);
                //user.carts.Remove(cart);
                dbContext.carts.Remove(cart);
                dbContext.SaveChanges();
            }
            else
            {

                Debug.WriteLine("did't found cart data");
            }

        }


        // 9) checkOut
        public void checkOutCartView(string username)
        {

            // all cart data transfer to the 

            //1. add data to purchaseHistory
            User user = dbContext.Users.FirstOrDefault(
                x => x.Username == username
                );

            var tupList = this.getCartViewList(username);
            List<int> QuantityList = tupList.Item1;
            List<Product> ProductList = tupList.Item2;
            // Console.WriteLine("backData :addResult={0},resultMessage={1}", tupList.Item1, tupList.Item2);

            for (int i = 0; i < ProductList.Count(); i++)
            {
                Product product = ProductList[i];
                int quantity = QuantityList[i];

                for (int j = 0; j < quantity; j++)
                {

                    string ACStr = "a-c" + Guid.NewGuid().ToString();
                    PurchaseHistory Purchase = new PurchaseHistory
                    {
                        PurchaseDate = DateTime.Now,
                        ActivationCode = ACStr
                    };

                    user.purHistories.Add(Purchase);
                    product.PurHistories.Add(Purchase);

                    dbContext.SaveChanges();
                }


            }

            //2. delete data from cart table
            List<Cart> carts = dbContext.carts.ToList();
            foreach (var item in carts)
            {
                dbContext.carts.Remove(item);
            }
            dbContext.SaveChanges();
        }


        //10) getPurchaseHis
        public List<PurchasesItem> getPurchaseHistory(Guid userId)
        {

            List<PurchaseHistory> purchases = dbContext.purHistories.Where(
                x => x.user.Id == userId
                ).ToList();

            if (purchases != null)
            {
                var iter = from pur in purchases
                           group pur by pur.product into productGroup
                           select productGroup;

                List<PurchasesItem> PurchasesItems = new List<PurchasesItem>();

                foreach (var grp in iter)
                {
                    PurchasesItem purchasItem = new PurchasesItem();

                    purchasItem.Quantity = grp.Count();


                    Console.WriteLine("{0}", grp.Count());

                    foreach (var pur in grp)
                    {
                        purchasItem.product = pur.product;
                        purchasItem.PurchaseDate = pur.PurchaseDate;
                        purchasItem.ActivationCode.Add(pur.ActivationCode);
                        Console.WriteLine("{0}", pur.product.ProductName);
                    }

                    PurchasesItems.Add(purchasItem);
                }

                return PurchasesItems;
            }

            return new List<PurchasesItem>();
        }


        //seedUsers
        public void SeedUsersTable()
        {
            HashAlgorithm sha = SHA256.Create();

            string[] usernames = { "john", "jean", "james", "kate", "david", "crist" };

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
        public void SeedProductsTable()
        {

            dbContext.Add(new Product
            {
                ProductName = ".NET Charts",
                Description = "Brings powerful charting capabilities to your .NET applications.",
                Price = 99,
                imageUrl = "../Image/charts.png"
            });


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
