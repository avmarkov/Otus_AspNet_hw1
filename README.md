# Домашняя работа № 1

#### 1. Сделать форк репозитория для домашнего задания №1 в свой личный репозиторий (см. Описание, ссылка на репозиторий https://gitlab.com/devgrav/otus.teaching.promocodefactory.homework.base);
Форк Сделал

#### 2. Убедиться, что проект запускается. Для этого достаточно проверить работу шаблона, вызвав метод API через Swagger. Проект использует .NET Core 3.1, так что надо убедиться, что установлено SDK https://dotnet.microsoft.com/download/dotnet-core/3.1 и все собирается и запускается на вашей машине.
Проект запускается. Изменил проект для .NET Core 8:
<image src="images/Screen_OK.png" alt="result">

#### 3. Реализовать Create/Delete/Update методы в EmployeesController на основе материалов ближайших уроков модуля и дополнительных материалов. Методы должны также использовать репозиторий с данными в памяти, как в базовом примере, при этом состояние списка сотрудников должно сохраняться между разными запросами. В качестве результата необходимо отправить в чат с преподавателем ссылку на свою домашнюю работу.
Реализовал Create/Delete/Update методы в EmployeesController:

Метод Create:
```cs
/// <summary>
/// Создать нового сотрудника
/// </summary>
/// <param name="createEmployee"></param>
/// <returns></returns>
[HttpPost]
public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync([FromBody] EmployeeShortRequest employeeRequest)
{
    // Создаем объект класса сотрудник        
    Employee employee = new()
    {
        FirstName = employeeRequest.FirstName,
        LastName = employeeRequest.LastName,
        Email = employeeRequest.Email,
        AppliedPromocodesCount = 5,
        Roles = new List<Role>()
    };

    // Этот сотрудник пусть будет администаротором
    employee.Roles.Add(new Role()
    {
        Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
        Name = "Admin",
        Description = "Администратор",
    });

    // Создаем его в репозитарии
    var createdEmployee = await _employeeRepository.CreateAsync(employee);
    if (createdEmployee == null)
        return BadRequest();

    // Возвращаем созданного сотрудника
    var employeeModel = new EmployeeResponse()
    {
        Id = createdEmployee.Id,
        Email = createdEmployee.Email,
        Roles = createdEmployee.Roles.Select(x => new RoleItemResponse()
        {
            Name = x.Name,
            Description = x.Description
        }).ToList(),
        FullName = createdEmployee.FullName,
        AppliedPromocodesCount = createdEmployee.AppliedPromocodesCount
    };
    return employeeModel;
}
```


```cs
```


```cs
```