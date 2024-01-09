using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace MudBlazorTemplates1.Data
{
    public class DataManager
    {
        private const string TasksKey = "TasksList";
        private const string TaskIdKey = "TaskId";
        private readonly IJSRuntime _jsRuntime;

        public DataManager(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task AdicionarTask(Tarefa task)
        {
            task.Id = await GerarId();
            task.DataCriada = DateTime.Now;
            var tasks = await GetTasks();
            tasks.Add(task);
            SalvarTasks(tasks);
            SalvarUltimaTaskId(task.Id);
        }

        public async Task EditarTask(Tarefa task)
        {
            var tasks = await GetTasks();
            var taskDirecionada = tasks.First(w => w.Id == task.Id);
            taskDirecionada.Descricao = task.Descricao;
            taskDirecionada.IsCompleted = task.IsCompleted;
            taskDirecionada.DataMarcada = task.DataMarcada;
            taskDirecionada.TaskStatusIndex = task.TaskStatusIndex;
            SalvarTasks(tasks);
        }

        public async Task ExcluirTask(Tarefa task)
        {
            var tasks = await GetTasks();
            var taskDirecionada = tasks.First(w => w.Id == task.Id);
            tasks.Remove(taskDirecionada);
            SalvarTasks(tasks);
        }

        private async Task<long> GerarId()
        {
            return await GetFromLocalStorage<long?>(TaskIdKey) + 1 ?? 0;
        }

        private void SalvarUltimaTaskId(long taskId)
        {
            SaveToLocalStorage(TaskIdKey, taskId);
        }

        public void SalvarTasks(List<Tarefa> tasks)
        {
            SaveToLocalStorage(TasksKey, tasks);
        }

        public async Task<List<Tarefa>> GetTasks()
        {
            return await GetFromLocalStorage<List<Tarefa>>(TasksKey) ?? new List<Tarefa>();
        }

        private async Task<T?> GetFromLocalStorage<T>(string key)
        {
            string? serializedValue = await ReadLocalStorage(key);

            if(serializedValue != null)
                return JsonConvert.DeserializeObject<T?>(serializedValue);

            return default;
        }

        private void SaveToLocalStorage<T>(string key, T value)
        {
            WriteLocalStorage(key, JsonConvert.SerializeObject(value));
        }

        private async Task<string?> ReadLocalStorage(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        private void WriteLocalStorage(string key, string value)
        {
            _jsRuntime.InvokeAsync<object>("localStorage.setItem", key, value);
        }
    }
}
