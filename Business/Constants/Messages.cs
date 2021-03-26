using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed = "Ürünler listelendi";
        public static string ProductCountOfCategoryError = "En fazla 15 ürün olabilir";
        public static string ProductNameAlreadyExists = "Ürün ismi zaten var";
        public static string CategoryLimitExceded = "Kategori sayısı 15'den fazla";
        public static string AuthorizationDenied ="Yetkiniz yok";
    }
}
