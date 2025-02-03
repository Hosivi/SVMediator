# SVMediator

## ✨ Mediador para todos los proyectos

SVMediator es una implementación personal del **Mediator Pattern**, similar a [MediatR](https://github.com/jbogard/MediatR). Este proyecto fue desarrollado con el propósito de comprender su funcionamiento y demostrar mis habilidades en **arquitectura de software y patrones de diseño**.

---

## 🛠️ Instalación

Actualmente, SVMediator no está disponible como un paquete NuGet. Para usarlo en tu proyecto:

1. Clona este repositorio o descarga el código fuente.
2. Agrega el proyecto **SVMediator** a tu solución en Visual Studio.
3. Agrega una referencia al proyecto **SVMediator** desde tu aplicación.

```sh
# Clonar el repositorio
git clone https://github.com/TU_USUARIO/SVMediator.git
```

---

## 🔧 Uso Básico

### 1. Definir un Comando o Consulta
```csharp
public class ObtenerProductoQuery : IRequest<Producto>
{
    public int Id { get; set; }
}
```

### 2. Implementar un Manejador
```csharp
public class ObtenerProductoHandler : IRequestHandler<ObtenerProductoQuery, Producto>
{
    private readonly IRepositorioProductos _repositorio;
    
    public ObtenerProductoHandler(IRepositorioProductos repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Producto> Handle(ObtenerProductoQuery request)
    {
        return await _repositorio.ObtenerPorId(request.Id);
    }
}
```

### 3. Usar el Mediador
```csharp
builder.Services.AddSVMediator(ServiceLifetime.Transient, types.ToArray());
```

---

## 🌐 Integración con .NET Dependency Injection
Para registrar SVMediator en una aplicación **ASP.NET Core**:

```csharp
services.AddSVMediator();
```

---

## 👀 Características
- Implementación ligera del **Mediator Pattern**.
- Separación clara de responsabilidades.
- Compatible con .NET 7 y superior.
- Integración con **Dependency Injection**.

---

## 🚀 Propósito del Proyecto
Este proyecto fue desarrollado con el objetivo de:
- **Aprender y aplicar el patrón Mediator** en un entorno real.
- **Mejorar mis habilidades en arquitectura de software**.
- **Demostrar mis conocimientos en C# y .NET** en mi portafolio de GitHub para oportunidades laborales.

---

## ⭐ Contribuciones
Este es un proyecto personal, pero si tienes sugerencias, puedes abrir un **pull request** o crear un **issue** con tus ideas.

---

## 👋 Contacto
Cualquier duda o sugerencia, puedes contactarme a través de **[GitHub Issues](https://github.com/TU_USUARIO/SVMediator/issues)** o en mi perfil de **GitHub**.

---

### © 2025 - SVMediator - Licencia MIT
