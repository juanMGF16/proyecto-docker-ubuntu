namespace Entity.DTOs.System.Item
{
        public class ItemConsultCategoryDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; }
            public string StateItemName { get; set; }
            //public int StateItemId { get; set; }
        }
    }