# �������� ������ � 1

#### 1. ������� ���� ����������� ��� ��������� ������� �1 � ���� ������ ����������� (��. ��������, ������ �� ����������� https://gitlab.com/devgrav/otus.teaching.promocodefactory.homework.base);
���� ������

#### 2. ���������, ��� ������ �����������. ��� ����� ���������� ��������� ������ �������, ������ ����� API ����� Swagger. ������ ���������� .NET Core 3.1, ��� ��� ���� ���������, ��� ����������� SDK https://dotnet.microsoft.com/download/dotnet-core/3.1 � ��� ���������� � ����������� �� ����� ������.
������ �����������. ������� ������ ��� .NET Core 8:
<image src="images/Screen_OK.png" alt="result">

#### 3. ����������� Create/Delete/Update ������ � EmployeesController �� ������ ���������� ��������� ������ ������ � �������������� ����������. ������ ������ ����� ������������ ����������� � ������� � ������, ��� � ������� �������, ��� ���� ��������� ������ ����������� ������ ����������� ����� ������� ���������. � �������� ���������� ���������� ��������� � ��� � �������������� ������ �� ���� �������� ������.
���������� Create/Delete/Update ������ � EmployeesController:

����� Create:
```cs
/// <summary>
/// ������� ������ ����������
/// </summary>
/// <param name="createEmployee"></param>
/// <returns></returns>
[HttpPost]
public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync([FromBody] EmployeeShortRequest employeeRequest)
{
    // ������� ������ ������ ���������        
    Employee employee = new()
    {
        FirstName = employeeRequest.FirstName,
        LastName = employeeRequest.LastName,
        Email = employeeRequest.Email,
        AppliedPromocodesCount = 5,
        Roles = new List<Role>()
    };

    // ���� ��������� ����� ����� ����������������
    employee.Roles.Add(new Role()
    {
        Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
        Name = "Admin",
        Description = "�������������",
    });

    // ������� ��� � �����������
    var createdEmployee = await _employeeRepository.CreateAsync(employee);
    if (createdEmployee == null)
        return BadRequest();

    // ���������� ���������� ����������
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

����� Delete:
```cs
/// <summary>
/// ������� ����������
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

����� Update:
```cs

/// <summary>
/// �������� ������ ����������
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
    // ���������� �����������  ����������
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

������ Create/Delete/Update � InMemoryRepository:
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