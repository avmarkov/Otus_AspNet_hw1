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

Метод Delete:
```cs
/// <summary>
/// Удалить сотрудника
/// </summary>
/// <param name="createEmployee"></param>
/// <returns></returns>
[HttpDelete("{id:guid}")]
public async Task<ActionResult<EmployeeResponse>> Delete(Guid id)
{
    var employee = await _employeeRepository.Delete(id);
    if (employee == null)
    {
        return NotFound();
    }
    var employeeModel = new EmployeeResponse()
    {
        Id = employee.Id,
        Email = employee.Email,
        Roles = employee.Roles.Select(x => new RoleItemResponse()
        {
            Name = x.Name,
            Description = x.Description
        }).ToList(),
        FullName = employee.FullName,
        AppliedPromocodesCount = employee.AppliedPromocodesCount
    };
    return Ok(employee);
}
```

Метод Update:
```cs

/// <summary>
/// Обновить данные сотрудника
/// </summary>
/// <param name="UpdateEmployee"></param>
/// <returns></returns>
[HttpPut]
public async Task<ActionResult<EmployeeResponse>> Update(Guid id, [FromBody] EmployeeShortRequest employeeRequest)
{
    var findedEmployee = await _employeeRepository.GetByIdAsync(id);
    if (findedEmployee == null)
    {
        return NotFound();
    }

    //employee.FirstName = employeeRequest.FirstName;
    //employee.LastName = employeeRequest.LastName;
    //employee.Email = employeeRequest.Email;

    Employee employee = new()
    {
        Id = findedEmployee.Id,
        FirstName = employeeRequest.FirstName,
        LastName = employeeRequest.LastName,
        Email = employeeRequest.Email,
        AppliedPromocodesCount = findedEmployee.AppliedPromocodesCount,
        Roles = findedEmployee.Roles
    };

    var updatedEmployee = await _employeeRepository.Update(employee);
    // Возвращаем измененного  сотрудника
    var employeeModel = new EmployeeResponse()
    {
        Id = updatedEmployee.Id,
        Email = updatedEmployee.Email,
        Roles = updatedEmployee.Roles.Select(x => new RoleItemResponse()
        {
            Name = x.Name,
            Description = x.Description
        }).ToList(),
        FullName = updatedEmployee.FullName,
        AppliedPromocodesCount = updatedEmployee.AppliedPromocodesCount
    };
    return Ok(employeeModel);
}
```

Методы Create/Delete/Update в InMemoryRepository:
```cs
public Task<T> CreateAsync(T entity)
{
    entity.Id = Guid.NewGuid();
    var dataList = Data.ToList();
    dataList.Add(entity);
    Data = dataList;
    return Task.FromResult(entity);

}

public Task<T> Delete(Guid id)
{
    T entity = Data.FirstOrDefault(x => x.Id == id);
    Data = Data.Where(x => x.Id != id);
    return Task.FromResult(entity);
}

public Task<T> Update(T entity)
{
    var dataList = Data.ToList();
    var data = dataList.FirstOrDefault(x => x.Id == entity.Id);

    if (data == null)
        return null;

    int index = dataList.IndexOf(data);
    if (index == -1)
    {
        return null;
    }

    dataList[index] = entity;

    Data = dataList;           
    return Task.FromResult(entity);
}
```