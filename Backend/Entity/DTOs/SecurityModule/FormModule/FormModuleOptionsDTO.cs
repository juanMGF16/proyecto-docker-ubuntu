﻿namespace Entity.DTOs.SecurityModule.FormModule
{
    public class FormModuleOptionsDTO
    {
        public int Id { get; set; }
        public bool Active { get; set; }

        public int FormId { get; set; }
        public int ModuleId { get; set; }
    }
}