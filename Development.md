# ToDoAPI - Udviklings Roadmap

## ?? Projekt Oversigt
Udvidelse af eksisterende Todo-API til et fuldt projekt- og opgavestyringssystem. Systemet skal kunne håndtere flere projekter, tildele mennesker til projekter, oprette tasks under projekter, og hierarkiske todos under hver task.

---

## ?? Mål
- ? Opret hierarkisk datastruktur: Projekter ? Tasks ? Todos
- ? Tilføj mennesker/team-medlemmer til projekter
- ? Implementer fuldt CRUD (Create, Read, Update, Delete) for alle entities
- ? Dokumenter API med Swagger
- ? Sikre relationer og konsistens i databasen

---

## ?? Udviklings Forlæb (Phase-by-Phase)

### **Phase 1: Data Model & Database** 
*Tidsestimation: 2-3 timer*

**Mål**: Opret database entities og konfigurer Entity Framework relationer.

**Tasks**:
1. Opret `Project.cs` entity med properties: Id, Name, Description, CreatedDate
2. Opret `Person.cs` entity med properties: Id, Name, Email, ProjectId
3. Opret `Task.cs` entity med properties: Id, Title, Description, Status enum, ProjectId, AssignedToId
4. Opdater `Todo.cs` - tilføj TaskId foreign key
5. Opdater `TodoDb.cs` - tilføj DbSet<Project>, DbSet<Person>, DbSet<Task>
6. Konfigurer relationer i DbContext:
   - Project.Person (1:N)
   - Project.Tasks (1:N)
   - Person.Tasks (1:N)
   - Task.Todos (1:N)

**Når det er gjort**: Test ved at køre `dotnet ef migrations add InitialCreate` (hvis database provider skifter fra In-Memory)

---

### **Phase 2: DTOs (Data Transfer Objects)**
*Tidsestimation: 1-2 timer*

**Mål**: Opret record-baserede DTOs for serialisering til API responses.

**Tasks**:
1. Opret `ProjectDTO.cs` - record med Id, Name, Description
2. Opret `PersonDTO.cs` - record med Id, Name, Email
3. Opret `TaskDTO.cs` - record med Id, Title, Description, Status, ProjectId, AssignedToId
4. Opdater `TodoItemDTO.cs` - tilføj TaskId property
5. Tilføj konstruktorer der mapper fra entity til DTO

**Når det er gjort**: Test at mapping fungerer uden fejl

---

### **Phase 3: Project Endpoints**
*Tidsestimation: 2 timer*

**Mål**: Implementer fuldt CRUD for projekter.

**Tasks**:
1. Opret `Endpoints/ProjectApiEndpoints.cs`
2. Implementer endpoints:
   - `GET /api/projects` - hent alle
   - `GET /api/projects/{id}` - hent en
   - `POST /api/projects` - opret ny (med validation)
   - `PUT /api/projects/{id}` - opdater
   - `DELETE /api/projects/{id}` - slet
3. Tilføj error handling (404, 400, osv.)
4. Registrer endpoints i `Program.cs`

**Test med Swagger**: Prøv alle CRUD operationer

---

### **Phase 4: Person Endpoints**
*Tidsestimation: 2 timer*

**Mål**: Implementer fuldt CRUD for mennesker/team-medlemmer.

**Tasks**:
1. Opret `Endpoints/PersonApiEndpoints.cs`
2. Implementer endpoints:
   - `GET /api/projects/{projectId}/Person` - hent alle i projekt
   - `GET /api/Person/{id}` - hent en
   - `POST /api/projects/{projectId}/Person` - tilføj til projekt
   - `PUT /api/Person/{id}` - opdater
   - `DELETE /api/Person/{id}` - slet
3. Tilføj validation (sikr at ProjectId findes)
4. Registrer endpoints i `Program.cs`

**Test**: Opret projekt, tilføj mennesker til det

---

### **Phase 5: Task Endpoints**
*Tidsestimation: 2-3 timer*

**Mål**: Implementer fuldt CRUD for tasks.

**Tasks**:
1. Opret `Endpoints/TaskApiEndpoints.cs`
2. Implementer endpoints:
   - `GET /api/projects/{projectId}/tasks` - hent alle i projekt
   - `GET /api/tasks/{id}` - hent en
   - `POST /api/projects/{projectId}/tasks` - opret ny
   - `PUT /api/tasks/{id}` - opdater (inkl. status)
   - `DELETE /api/tasks/{id}` - slet
3. Validering: ProjectId skal findes, AssignedToId skal være valid Person
4. Status enum validation
5. Registrer endpoints

**Test**: Opret task under projekt, tildel person

---

### **Phase 6: Todo Endpoints Udvidelse**
*Tidsestimation: 1-2 timer*

**Mål**: Opdater Todo endpoints til at arbejde under Tasks hierarkisk.

**Tasks**:
1. Opdater `TodoApiEndpoints.cs`:
   - `GET /api/tasks/{taskId}/todos` - hent todos for task
   - `POST /api/tasks/{taskId}/todos` - opret todo under task
   - `PUT /api/todos/{id}` - opdater (inkl. TaskId check)
   - `DELETE /api/todos/{id}` - slet
2. Sikr TaskId validering
3. Registrer nye endpoints

**Test**: Opret task, tilføj todos under den

---

### **Phase 7: Integration & Test**

**Mål**: Test hele systemet end-to-end.

**Tasks**:
1. Test fuldt workflow:
   - Opret projekt
   - Tilføj mennesker
   - Opret task
   - Tildel person
   - Opret todos under task
   - Markér som færdig
2. Test fejlscenarier:
   - Slet projekt ? validering af cascade
   - Ugyldig data ? 400 responses
   - Ikke-eksisterende ressourcer ? 404
3. Swagger dokumentation fuldstændig
4. Git commit: "feat: complete API expansion"

---

## ?? Vigtige Principper
- **Validering**: Alle inputs skal valideres før database-operationer
- **Error Handling**: Brugbar error messages (400, 404, 500, osv.)
- **RESTful Design**: Korrekte HTTP metoder og status codes
- **Relationer**: Sikr data integritet (FK constraints)
- **DTOs**: Mapper altid entities ? DTOs i responses

---

## ? Acceptance Criteria
- [ ] Alle 4 entities (Project, Person, Task, Todo) funktionerer
- [ ] CRUD komplette for alle ressourcer
- [ ] Relationer virker korrekt (hierarki bevares)
- [ ] Error handling på alle endpoints
- [ ] Swagger dokumentation er opdateret
- [ ] Kan teste alle endpoints manuelt i Swagger UI
- [ ] Ingen compilation errors
- [ ] Git commits for hver phase

---