namespace MudBlazorTemplates1.Data
{
    public partial class Tarefa
    {
        public long Id { get; set; }

        public string Titulo { get; set; } = "Nova Tarefa";

        public string Descricao { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
        
        public DateTime DataCriada { get; set; }

        public DateTime? DataMarcada { get; set; } = DateTime.Now;

        public int? TaskStatusIndex { get; set; } = 0;
    }
}
