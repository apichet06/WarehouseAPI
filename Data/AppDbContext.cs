using Microsoft.EntityFrameworkCore;
using ServiceRepairComputer.Models;
using Warehouse_API.Models;

namespace Warehouse_API.Data
{
    public class AppDBContext:DbContext
    {
       public  AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        
       public DbSet<Division> Divisions { get; set; }
       public DbSet<IncomingStock> IncomingStocks { get; set;}
       public DbSet<Picking_goodsDetail> Picking_GoodsDetails { get; set;}
       public DbSet<Position> Positions { get; set; }
       public DbSet<Product> Products { get; set; }
       public DbSet<Users> Users { get; set; }
       public DbSet<ApprovalRequest> Approvals { get; set; } 
       public DbSet<ProductType> ProductTypes { get; set; }
       public DbSet<InventoryRequest> InventoryRequests { get; set; }

     
    }
}
