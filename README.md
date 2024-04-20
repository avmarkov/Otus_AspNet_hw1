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


```cs
```


```cs
```