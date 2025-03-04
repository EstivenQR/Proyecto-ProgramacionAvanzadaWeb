 Proyecto-ProgramacionAvanzadaWeb

 1. Integrantes finales del grupo.
    -Esteven Francisco Quirós Rodríguez 
    -Joshua Jafeth Mejías Castellón 
    -Leonardo Madrigal Sánchez 
    -Maria Paula Cordero Segura. 

 3. Especificación básica del proyecto:

	a. Arquitectura del proyecto:
    - ASP.NET CORE WEB APP MVC (Model-View-Controller): Divide la aplicación en modelos, vistas y controladores.
    - ASP.NET CORE WEB API:
     
	b. Libraries o paquetes de nuget ualizados.
    - Microsoft.EntityFrameworkCore.SqlServer:Para manejar bases de datos SQL Server con Entity Framework Core.
    - Microsoft.EntityFrameworkCore.Design: Permite generar código automáticamente para modelos de base de datos.
    - Microsoft.EntityFrameworkCore.Tools: Se usa en la terminal para administrar migraciones y actualizar bases de datos.
    - Microsoft.VisualStudio.Web.CodeGeneration.Design: Se emplea para agilizar el desarrollo con generación automática de archivos en proyectos MVC.
    - Swashbuckle.AspNetCore: Genera una interfaz gráfica en /swagger para visualizar y probar la API
    - Microsoft.AspNetCore.OpenApi: Facilita la generación de documentación automática basada en atributos de los controladores y modelos.
     
	c. Principios de SOLID y patrones de diseño utilizados

    1. Single Responsibility Principle (SRP)
      - Este principio establece que cada clase debe tener una única responsabilidad
    
      Aplicación:
      - Controladores deben ser responsables de manejar la lógica de las solicitudes y responder con vistas o datos. No deben encargarse de la lógica de negocio compleja, la cual debe residir en otras partes          del sistema.
      - Modelos deben representar solo los datos o entidades de tu aplicación y no deben tener lógica de negocio o lógica de acceso a datos.

    2. Open/Closed Principle (OCP)
       - Este principio establece que las clases deben estar abiertas para la extensión pero cerradas para la modificación.
    
      Aplicación:
      - Si se necesitara agregar una nueva funcionalidad, como una nueva forma de buscar libros, no se necesitas cambiar el controlador existente. Se puede extender el sistema agregando un nuevo método o   
        controlador para manejar la nueva funcionalidad. Esto permite que el sistema sea más flexible y fácil de mantener.

    3. Liskov Substitution Principle (LSP)
       - Este principio establece que las clases derivadas deben poder ser sustituidas por sus clases base sin alterar el comportamiento del sistema.

       Aplicación:
       - Si tenemos una función que maneja libros en nuestro sistema, esta debe poder manejar tanto libros físicos como libros digitales sin ningún problema, independientemente de su tipo específico.
       
    4. Interface Segregation Principle (ISP)
       - Este principio dice que los clientes no deben estar obligados a depender de interfaces que no utilizan. En otras palabras, cada interfaz debe ser específica y contener solo los métodos que son 
         relevantes para los usuarios de esa interfaz.

        Aplicación:
       - Si el sistema tiene varios tipos de operaciones (por ejemplo, leer libros, agregar libros, eliminar libros), en lugar de tener una sola interfaz que realice todas las acciones, se debería dividir en           interfaces más pequeñas y específicas. Por ejemplo, podría haber una interfaz dedicada únicamente a la lectura de libros y otra para la creación o eliminación de libros.

    5. Dependency Inversion Principle (DIP)
        Este principio establece que las clases de alto nivel no deben depender de clases de bajo nivel, sino de abstracciones (como interfaces). Además, las abstracciones no deben depender de detalles     
        concretos, los detalles concretos deben depender de las abstracciones.

        Aplicación:
        -  En lugar de que el controlador dependa directamente de clases concretas (como una implementación específica de acceso a base de datos), debería depender de interfaces que definan el comportamiento necesario. Esto permite una mayor flexibilidad 	y facilidad de mantenimiento. El controlador puede interactuar con cualquier clase que implemente la interfaz sin importar la tecnología o la implementación concreta detrás de la interfaz.


      Patrones de Diseño utilizados:
      - Patrón Factory: El patrón Repository es comúnmente utilizado con Entity Framework Core cuando usas migraciones. Este patrón tiene como objetivo proporcionar una capa de abstracción sobre el acceso a datos.
      - Patrón Unit of Work:El contexto de base de datos (DbContext) puede ser considerado como una implementación de este patrón. Cuando realizas varias operaciones (como crear, actualizar y     
        eliminar) en varias entidades, el Unit of Work garantiza que estas operaciones se gestionen dentro de una única transacción. EF Core mantiene un seguimiento de los cambios en las entidades, y cuando 
        ejecutas SaveChanges(), se aseguran de que los cambios se persistan correctamente en la base de datos.
      - Patrón ViewModel: Es un patrón muy común en ASP.NET Core MVC. El ViewModel es una clase que contiene la información que se va a mostrar en la vista. A diferencia del modelo, que refleja la estructura de la base de datos, el ViewModel está 	 
        diseñado específicamente para la vista y puede incluir datos agregados o transformados. Esto mantiene las vistas desacopladas del modelo de datos y facilita la adaptación de la interfaz.
      - Patrón Repository: Este patrón abstrae el acceso a los datos, proporcionando una capa intermedia entre el controlador y la fuente de datos (por ejemplo, una base de datos). Los 
        controladores interactúan con  el repositorio, que se encarga de las operaciones de lectura/escritura.


        









    
