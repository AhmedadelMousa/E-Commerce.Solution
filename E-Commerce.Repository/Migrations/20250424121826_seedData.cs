using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Repository.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
            -- Delete from AspNetUserRoles
            DELETE FROM AspNetUserRoles;

            -- Delete from AspNetUsers
            DELETE FROM AspNetUsers;

            -- Delete from AspNetRoles
            DELETE FROM AspNetRoles;

            -- Delete from MakeReviews
            DELETE FROM MakeReviews;

            -- Delete from deliveryMethods
            DELETE FROM deliveryMethods;

            -- Delete from Products
            DELETE FROM Products;

            -- Delete from Categories
            DELETE FROM Categories;
            INSERT INTO [dbo].[Categories] (Id, Name, Description)
            VALUES
                ('a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', 'Men''s Clothing', 'Clothing items for men including shirts, pants, and outerwear'),
                ('b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e', 'Women''s Clothing', 'Clothing items for women including tops, dresses, and skirts'),
                ('c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f', 'Children''s Clothing', 'Clothing items for children aged 0-12 years'),
                ('d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', 'Teen Clothing', 'Clothing items for teenagers aged 13-19 years'),
                ('e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', 'Unisex Clothing', 'Clothing items suitable for all genders');


                -- Insert all product records
                INSERT INTO Products (Id, Name, Description, PictureUrl, Size, Colors, StockQuantity, Price, CategoryId, CreatedAt) VALUES
                ('3a4b5c6d-7e8f-4a9b-8c9d-0e1f2a3b4c5d', 'Classic White T-Shirt', 'A comfortable and stylish white t-shirt.', 'Img/Products/ClassicWhiteT-Shirt.jpg', 'M', 'White', 50, 19.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-03-15 08:30:00'),
                ('4b5c6d7e-8f9a-4b8c-9d0e-1f2a3b4c5d6e', 'Black Hoodie', 'A warm and cozy black hoodie.', 'Img/Products/BlackHoodie.jpg', 'L', 'Black', 30, 39.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-22 14:15:00'),
                ('5c6d7e8f-9a0b-4c9d-0e1f-2a3b4c5d6e7f', 'Striped Polo Shirt', 'A casual striped polo shirt.', 'Img/Products/StripedPoloShirt.jpg', 'S', 'Blue, White', 40, 29.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-04-05 09:45:00'),
                ('6d7e8f9a-0b1c-4d0e-1f2a-3b4c5d6e7f8g', 'Red Flannel Shirt', 'A warm red flannel shirt for cold days.', 'Img/Products/RedFlannelShirt.jpg', 'XL', 'Red', 20, 49.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-03-28 16:20:00'),
                ('7e8f9a0b-1c2d-4e1f-2a3b-4c5d6e7f8g9h', 'Graphic Print T-Shirt', 'A trendy t-shirt with a unique graphic print.', 'Img/Products/GraphicPrintT-Shirt.jpg', 'M', 'Black, White', 60, 24.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-04-12 11:10:00'),
                ('8f9a0b1c-2d3e-4f2a-3b4c-5d6e7f8g9h0i', 'Lightweight Jacket', 'A lightweight jacket for spring weather.', 'Img/Products/LightweightJacket.jpg', 'L', 'Green', 25, 59.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-02-18 13:25:00'),
                ('9a0b1c2d-3e4f-4a3b-4c5d-6e7f8g9h0i1j', 'Denim Jeans', 'Classic blue denim jeans.', 'Img/Products/DenimJeans.jpg', '32', 'Blue', 35, 69.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-03-08 10:40:00'),
                ('0b1c2d3e-4f5a-4b4c-5d6e-7f8g9h0i1j2k', 'Running Sneakers', 'Comfortable sneakers for running.', 'Img/Products/RunningSneakers.jpg', '10', 'Black, White', 50, 89.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-03-25 15:30:00'),
                ('1c2d3e4f-5a6b-4c5d-6e7f-8g9h0i1j2k3l', 'Leather Jacket', 'A stylish leather jacket for a bold look.', 'Img/Products/LeatherJacket.jpg', 'M', 'Black', 15, 199.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-04-15 09:20:00'),
                ('2d3e4f5a-6b7c-4d6e-7f8g-9h0i1j2k3l4m', 'Cargo Shorts', 'Durable cargo shorts with multiple pockets.', 'Img/Products/CargoShorts.jpg', '34', 'Khaki', 40, 49.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-03-30 14:50:00'),
                ('3e4f5a6b-7c8d-4e7f-8g9h-0i1j2k3l4m5n', 'Cotton Tank Top', 'A lightweight cotton tank top for summer.', 'Img/Products/CottonTankTop.jpg', 'S', 'Gray', 55, 14.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-04-18 08:15:00'),
                ('4f5a6b7c-8d9e-4f8g-9h0i-1j2k3l4m5n6o', 'Wool Coat', 'A warm wool coat for winter.', 'Img/Products/WoolCoat.jpg', 'L', 'Navy', 10, 249.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-02-12 11:30:00'),
                ('5a6b7c8d-9e0f-4g9h-0i1j-2k3l4m5n6o7p', 'Chino Pants', 'Classic chino pants for a smart casual look.', 'Img/Products/ChinoPants.jpg', '36', 'Beige', 30, 59.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-04-10 13:45:00'),
                ('6b7c8d9e-0f1a-4h0i-1j2k-3l4m5n6o7p8q', 'Hiking Boots', 'Durable hiking boots for outdoor adventures.', 'Img/Products/HikingBoots.jpg', '11', 'Brown', 20, 129.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-02-08 10:20:00'),
                ('7c8d9e0f-1a2b-4i1j-2k3l-4m5n6o7p8q9r', 'Puffer Jacket', 'A warm puffer jacket for cold weather.', 'Img/Products/PufferJacket.jpg', 'XL', 'Black', 18, 179.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-03-18 16:10:00'),
                ('8d9e0f1a-2b3c-4j2k-3l4m-5n6o7p8q9r0s', 'Slim Fit Jeans', 'Slim fit jeans for a modern look.', 'Img/Products/SlimFitJeans.jpg', '30', 'Black', 25, 79.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-22 09:35:00'),
                ('9e0f1a2b-3c4d-4k3l-4m5n-6o7p8q9r0s1t', 'Canvas Sneakers', 'Casual canvas sneakers for everyday wear.', 'Img/Products/CanvasSneakers.jpg', '9', 'White', 45, 49.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-03-10 14:25:00'),
                ('0f1a2b3c-4d5e-4l4m-5n6o-7p8q9r0s1t2u', 'V-Neck Sweater', 'A soft v-neck sweater for a cozy feel.', 'Img/Products/V-NeckSweater.jpg', 'M', 'Navy', 22, 69.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-05 12:40:00'),
                ('1a2b3c4d-5e6f-4m5n-6o7p-8q9r0s1t2u3v', 'Trench Coat', 'A classic trench coat for a sophisticated look.', 'Img/Products/TrenchCoat.jpg', 'L', 'Beige', 12, 299.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-04-05 10:15:00'),
                ('2b3c4d5e-6f7g-4n6o-7p8q-9r0s1t2u3v4w', 'Jogger Pants', 'Comfortable jogger pants for casual wear.', 'Img/Products/JoggerPants.jpg', 'M', 'Gray', 35, 44.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-03-15 15:50:00'),
                ('3c4d5e6f-7g8h-4o7p-8q9r-0s1t2u3v4w5x', 'Slip-On Loafers', 'Elegant slip-on loafers for a polished look.', 'Img/Products/Slip-OnLoafers.jpg', '10', 'Brown', 28, 99.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-03-05 08:30:00'),
                ('4d5e6f7g-8h9i-4p8q-9r0s-1t2u3v4w5x6y', 'Henley Shirt', 'A casual henley shirt for a relaxed style.', 'Img/Products/HenleyShirt.jpg', 'L', 'Green', 40, 34.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-28 13:20:00'),
                ('5e6f7g8h-9i0j-4q9r-0s1t-2u3v4w5x6y7z', 'Bomber Jacket', 'A trendy bomber jacket for a cool look.', 'Img/Products/BomberJacket.jpg', 'M', 'Olive', 20, 149.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-04-15 11:45:00'),
                ('6f7g8h9i-0j1k-4r0s-1t2u-3v4w5x6y7z8a', 'Cargo Pants', 'Stylish cargo pants with utility pockets.', 'Img/Products/CargoPants.jpg', '32', 'Black', 30, 54.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-10 09:10:00'),
                ('7g8h9i0j-1k2l-4s1t-2u3v-4w5x6y7z8a9b', 'Espadrilles', 'Lightweight espadrilles for summer.', 'Img/Products/Espadrilles.jpg', '8', 'Blue', 15, 39.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-03-22 14:30:00'),
                ('8h9i0j1k-2l3m-4t2u-3v4w-5x6y7z8a9b0c', 'Long Sleeve Tee', 'A comfortable long sleeve t-shirt.', 'Img/Products/LongSleeveTee.jpg', 'M', 'Black', 50, 24.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-03-20 10:50:00'),
                ('9i0j1k2l-3m4n-4u3v-4w5x-6y7z8a9b0c1d', 'Parka Jacket', 'A warm parka jacket for extreme cold.', 'Img/Products/ParkaJacket.jpg', 'XL', 'Green', 10, 199.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-03-08 16:20:00'),
                ('0j1k2l3m-4n5o-4v4w-5x6y-7z8a9b0c1d2e', 'Skinny Jeans', 'Skinny fit jeans for a sleek look.', 'Img/Products/SkinnyJeans.jpg', '28', 'Blue', 20, 69.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-25 12:15:00'),
                ('1k2l3m4n-5o6p-4w5x-6y7z-8a9b0c1d2e3f', 'Slip-On Sneakers', 'Casual slip-on sneakers for easy wear.', 'Img/Products/Slip-OnSneakers.jpg', '9', 'Black', 30, 59.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-04-12 09:40:00'),
                ('2l3m4n5o-6p7q-4x6y-7z8a-9b0c1d2e3f4g', 'Polo Shirt', 'A classic polo shirt for a smart casual look.', 'Img/Products/PoloShirt.jpg', 'L', 'White', 40, 29.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-18 14:10:00'),
                ('3m4n5o6p-7q8r-4y7z-8a9b-0c1d2e3f4g5h', 'Raincoat', 'A waterproof raincoat for rainy days.', 'Img/Products/Raincoat.jpg', 'M', 'Yellow', 15, 89.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-04-10 15:30:00'),
                ('4n5o6p7q-8r9s-4z8a-9b0c-1d2e3f4g5h6i', 'Corduroy Pants', 'Soft corduroy pants for a vintage look.', 'Img/Products/CorduroyPants.jpg', '34', 'Brown', 25, 64.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-08 11:25:00'),
                ('5o6p7q8r-9s0t-4a9b-0c1d-2e3f4g5h6i7j', 'Ankle Boots', 'Stylish ankle boots for a chic look.', 'Img/Products/AnkleBoots.jpg', '7', 'Black', 18, 109.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-03-20 13:50:00'),
                ('6p7q8r9s-0t1u-4b0c-1d2e-3f4g5h6i7j8k', 'Sport Sandals', 'Perfect for outdoor activities and summer trips.', 'Img/Products/SportSandals.jpg', '43', 'Black, Green', 35, 39.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-03-30 10:15:00'),
                ('7q8r9s0t-1u2v-4c1d-2e3f-4g5h6i7j8k9l', 'Windbreaker', 'A lightweight windbreaker for windy days.', 'Img/Products/Windbreaker.jpg', 'L', 'Blue', 20, 79.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-04-15 08:40:00'),
                ('8r9s0t1u-2v3w-4d2e-3f4g-5h6i7j8k9l0m', 'Carpenter Pants', 'Durable carpenter pants for work or casual wear.', 'Img/Products/CarpenterPants.jpg', '36', 'Khaki', 22, 59.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-12 15:20:00'),
                ('9s0t1u2v-3w4x-4e3f-4g5h-6i7j8k9l0m1n', 'Sandals', 'Comfortable sandals for summer.', 'Img/Products/Sandals.jpg', '8', 'Brown', 30, 34.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-04-18 12:30:00'),
                ('0t1u2v3w-4x5y-4f4g-5h6i-7j8k9l0m1n2o', 'Casual Slip-Ons', 'Lightweight and breathable slip-on shoes.', 'Img/Products/CasualSlip-Ons.jpg', '41', 'Gray', 18, 74.99, 'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h', '2025-02-22 09:50:00'),
                ('1u2v3w4x-5y6z-4g5h-6i7j-8k9l0m1n2o3p', 'Peacoat', 'A classic peacoat for a timeless look.', 'Img/Products/Peacoat.jpg', 'L', 'Navy', 12, 249.99, 'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g', '2025-03-10 16:10:00'),
                ('2v3w4x5y-6z7a-4h6i-7j8k-9l0m1n2o3p4q', 'Cargo Joggers', 'Stylish cargo joggers for a modern look.', 'Img/Products/CargoJoggers.jpg', 'M', 'Green', 28, 49.99, 'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d', '2025-02-05 14:40:00');

                -- Insert [dbo].[deliveryMethods]
                INSERT INTO [dbo].[deliveryMethods] (Id ,ShortName, Description, DeliveryTime, Cost)
                VALUES
                    ('4216A181-76D6-42A5-8C64-5A91635E4F83','UPS1', 'Fastest delivery time', '1-2 Days', 10),
                    ('0AA41070-5EE5-4CF2-8BD2-6B41747F1BCD','UPS2', 'Get it within 5 days', '2-5 Days', 5),
                    ('06E6C561-A697-4B2F-B144-EE0470B1C31A','UPS3', 'Slower but cheap', '5-10 Days', 2),
                    ('E2748252-A48D-45CC-A182-80D98B06DC11','FREE', 'Free! You get what you pay for', '1-2 Weeks', 0);

                -- Insert Roles
                INSERT INTO [dbo].[AspNetRoles] (Id, Name, NormalizedName, ConcurrencyStamp)
                VALUES
                    ('CD4D31DB-79F3-4870-B815-F74444DC4126', 'Admin', 'ADMIN', 'a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6'),
                    ('E0BC5FD6-6373-4E9D-8B0B-9D19800B6375', 'User', 'USER', 'a7b8c9d0-e1f2-3g4h-5i6j-k7l8m9n0o1p2'),
                    ('41D84D90-E21B-44DD-94D1-EB11DB03DF14', 'Guest', 'GUEST', 'a3b4c5d6-e7f8-9g0h-i1j2-k3l4m5n6o7p8');

                -- Insert Users
                INSERT INTO [dbo].[AspNetUsers] (
                    Id, DisplayName, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
                    PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
                    TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Role
                )
                VALUES
                    ('5e2f18e3-758b-4b8c-8988-daf3a4f457db', 'admin', 'admin@gmail.com', 'ADMIN@GMAIL.COM', 'admin@gmail.com',
                     'ADMIN@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEDiE1bpZ6WwqfyFYnmwqY+Y/zCJhgtgZBdDzSKkE6PivF7HTxdOLdSd+EWFbx91VMg==',
                     'PB2IHDWGR37TEK5Q62YBF2ZNZZEBUQSB', '23d7eebe-fc6f-4235-9264-1bac6148136e', NULL, 0, 0, NULL, 1, 0, 1),
                    ('6B2064B8-8197-4E75-A6FD-EC6C70FE1DDD', 'user', 'user@gmail.com', 'USER@GMAIL.COM', 'user@gmail.com',
                     'USER@GMAIL.COM', 0, 'AQAAAAIAAYagAAAAEDiE1bpZ6WwqfyFYnmwqY+Y/zCJhgtgZBdDzSKkE6PivF7HTxdOLdSd+EWFbx91VMg==',
                     'PB2IHDWGR37TEK5Q62YBF2ZNZZEBUQSB', '596936A9-6B4A-4CE9-9432-A6614E15EF9E', NULL, 0, 0, NULL, 1, 0, 0);

                -- Insert UserRoles
                INSERT INTO AspNetUserRoles ([UserId], [RoleId])
                VALUES
                    ('5e2f18e3-758b-4b8c-8988-daf3a4f457db', 'CD4D31DB-79F3-4870-B815-F74444DC4126'),
                    ('6B2064B8-8197-4E75-A6FD-EC6C70FE1DDD', 'E0BC5FD6-6373-4E9D-8B0B-9D19800B6375');

                
                -- Insert ProductReviews (only first 30 unique entries)
                INSERT INTO [dbo].[MakeReviews] (Id, Comment, NumberOfPoint, CreatedAt, ProductId, AppUserId)
                VALUES
                    ('3a4f8c2d-1b7e-4a9d-8c3f-6e2d1b5a9f4e', 'منتج رائع جدًا! الجودة ممتازة والتوصيل سريع.', 5, '2025-03-16T12:00:00Z', '3m4n5o6p-7q8r-4y7z-8a9b-0c1d2e3f4g5h', '6B2064B8-8197-4E75-A6FD-EC6C70FE1DDD'),
                    -- ... (include all 30 unique review entries here) ...
                    ('1b0a9c8d-7e6f-5a4b-3c2d-1e2f3a4b5c6d', 'تجربة شراء ممتازة، شكراً للبائع!', 5, '2025-02-05T13:00:00Z', '0t1u2v3w-4x5y-4f4g-5h6i-7j8k9l0m1n2o', '6B2064B8-8197-4E75-A6FD-EC6C70FE1DDD');

            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM AspNetUserRoles
                WHERE (UserId = '5e2f18e3-758b-4b8c-8988-daf3a4f457db' AND RoleId = 'CD4D31DB-79F3-4870-B815-F74444DC4126')
                   OR (UserId = '6B2064B8-8197-4E75-A6FD-EC6C70FE1DDD' AND RoleId = 'E0BC5FD6-6373-4E9D-8B0B-9D19800B6375');

                -- Delete Users
                DELETE FROM [dbo].[AspNetUsers]
                WHERE Id IN ('5e2f18e3-758b-4b8c-8988-daf3a4f457db', '6B2064B8-8197-4E75-A6FD-EC6C70FE1DDD');

                -- Delete Roles
                DELETE FROM AspNetRoles
                WHERE Id IN ('CD4D31DB-79F3-4870-B815-F74444DC4126', 'E0BC5FD6-6373-4E9D-8B0B-9D19800B6375', '41D84D90-E21B-44DD-94D1-EB11DB03DF14');

                -- Delete ProductReviews
                DELETE FROM [dbo].[MakeReviews]
                WHERE Id IN (
                    '3a4f8c2d-1b7e-4a9d-8c3f-6e2d1b5a9f4e',
                    -- ... (list all 30 review Ids) ...
                    '1b0a9c8d-7e6f-5a4b-3c2d-1e2f3a4b5c6d'
                );

                -- Delete ShippingOptions
                DELETE FROM [dbo].[deliveryMethods]
                WHERE ShortName IN ('UPS1', 'UPS2', 'UPS3', 'FREE');

                -- Delete all the specific products that were just added
                DELETE FROM Products
                WHERE Id IN (
                    '3a4b5c6d-7e8f-4a9b-8c9d-0e1f2a3b4c5d', -- Classic White T-Shirt
                    '4b5c6d7e-8f9a-4b8c-9d0e-1f2a3b4c5d6e', -- Black Hoodie
                    '5c6d7e8f-9a0b-4c9d-0e1f-2a3b4c5d6e7f', -- Striped Polo Shirt
                    '6d7e8f9a-0b1c-4d0e-1f2a-3b4c5d6e7f8g', -- Red Flannel Shirt
                    '7e8f9a0b-1c2d-4e1f-2a3b-4c5d6e7f8g9h', -- Graphic Print T-Shirt
                    '8f9a0b1c-2d3e-4f2a-3b4c-5d6e7f8g9h0i', -- Lightweight Jacket
                    '9a0b1c2d-3e4f-4a3b-4c5d-6e7f8g9h0i1j', -- Denim Jeans
                    '0b1c2d3e-4f5a-4b4c-5d6e-7f8g9h0i1j2k', -- Running Sneakers
                    '1c2d3e4f-5a6b-4c5d-6e7f-8g9h0i1j2k3l', -- Leather Jacket
                    '2d3e4f5a-6b7c-4d6e-7f8g-9h0i1j2k3l4m', -- Cargo Shorts
                    '3e4f5a6b-7c8d-4e7f-8g9h-0i1j2k3l4m5n', -- Cotton Tank Top
                    '4f5a6b7c-8d9e-4f8g-9h0i-1j2k3l4m5n6o', -- Wool Coat
                    '5a6b7c8d-9e0f-4g9h-0i1j-2k3l4m5n6o7p', -- Chino Pants
                    '6b7c8d9e-0f1a-4h0i-1j2k-3l4m5n6o7p8q', -- Hiking Boots
                    '7c8d9e0f-1a2b-4i1j-2k3l-4m5n6o7p8q9r', -- Puffer Jacket
                    '8d9e0f1a-2b3c-4j2k-3l4m-5n6o7p8q9r0s', -- Slim Fit Jeans
                    '9e0f1a2b-3c4d-4k3l-4m5n-6o7p8q9r0s1t', -- Canvas Sneakers
                    '0f1a2b3c-4d5e-4l4m-5n6o-7p8q9r0s1t2u', -- V-Neck Sweater
                    '1a2b3c4d-5e6f-4m5n-6o7p-8q9r0s1t2u3v', -- Trench Coat
                    '2b3c4d5e-6f7g-4n6o-7p8q-9r0s1t2u3v4w', -- Jogger Pants
                    '3c4d5e6f-7g8h-4o7p-8q9r-0s1t2u3v4w5x', -- Slip-On Loafers
                    '4d5e6f7g-8h9i-4p8q-9r0s-1t2u3v4w5x6y', -- Henley Shirt
                    '5e6f7g8h-9i0j-4q9r-0s1t-2u3v4w5x6y7z', -- Bomber Jacket
                    '6f7g8h9i-0j1k-4r0s-1t2u-3v4w5x6y7z8a', -- Cargo Pants
                    '7g8h9i0j-1k2l-4s1t-2u3v-4w5x6y7z8a9b', -- Espadrilles
                    '8h9i0j1k-2l3m-4t2u-3v4w-5x6y7z8a9b0c', -- Long Sleeve Tee
                    '9i0j1k2l-3m4n-4u3v-4w5x-6y7z8a9b0c1d', -- Parka Jacket
                    '0j1k2l3m-4n5o-4v4w-5x6y-7z8a9b0c1d2e', -- Skinny Jeans
                    '1k2l3m4n-5o6p-4w5x-6y7z-8a9b0c1d2e3f', -- Slip-On Sneakers
                    '2l3m4n5o-6p7q-4x6y-7z8a-9b0c1d2e3f4g', -- Polo Shirt
                    '3m4n5o6p-7q8r-4y7z-8a9b-0c1d2e3f4g5h', -- Raincoat
                    '4n5o6p7q-8r9s-4z8a-9b0c-1d2e3f4g5h6i', -- Corduroy Pants
                    '5o6p7q8r-9s0t-4a9b-0c1d-2e3f4g5h6i7j', -- Ankle Boots
                    '6p7q8r9s-0t1u-4b0c-1d2e-3f4g5h6i7j8k', -- Sport Sandals
                    '7q8r9s0t-1u2v-4c1d-2e3f-4g5h6i7j8k9l', -- Windbreaker
                    '8r9s0t1u-2v3w-4d2e-3f4g-5h6i7j8k9l0m', -- Carpenter Pants
                    '9s0t1u2v-3w4x-4e3f-4g5h-6i7j8k9l0m1n', -- Sandals
                    '0t1u2v3w-4x5y-4f4g-5h6i-7j8k9l0m1n2o', -- Casual Slip-Ons
                    '1u2v3w4x-5y6z-4g5h-6i7j-8k9l0m1n2o3p', -- Peacoat
                    '2v3w4x5y-6z7a-4h6i-7j8k-9l0m1n2o3p4q'  -- Cargo Joggers
                );

                -- Delete Categories
                DELETE FROM Categories
                WHERE Id IN (
                    'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d',
                    'b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e',
                    'c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f',
                    'd4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f8g',
                    'e5f6a7b8-c9d0-4e1f-2a3b-4c5d6e7f8g9h'
                );");
        }
    }
}
