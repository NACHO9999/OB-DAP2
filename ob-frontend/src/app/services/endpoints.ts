export enum UsersEndpoints { 
    LOGIN = 'api/sessions',
    LOGOUT = 'api/sessions/logout',
}

export enum AdminConstructoraEndpoints {
    BORRAR_EDIFICIO = 'adminconstructora/borrar-edificio',
    GET_EDIFICIO = 'adminconstructora/get-edificio',
    BORRAR_DEPTO = 'adminconstructora/borrar-depto',
    GET_EDIFICIOS_POR_ADMIN = 'adminconstructora/get-edificiosporadmin',
    CREAR_CONSTRUCTORA = 'adminconstructora/crear-constructora',
    EDITAR_EDIFICIO = 'adminconstructora/editar-edificio',
    CREAR_EDIFICIO = 'adminconstructora/crear-edificio',
    CREAR_DEPTO = 'adminconstructora/crear-depto',
    GET_DEPTO = 'adminconstructora/get-depto',
    EDITAR_DEPTO = 'adminconstructora/editar-depto',
    GET_EDIFICIOS_CON_ENCARGADOS = 'adminconstructora/get-edificios-con-encargados',
    GET_EDIFICIOS_SIN_ENCARGADOS = 'adminconstructora/get-edificios-sin-encargados',
    FILTRAR_POR_NOMBRE_EDIFICIO = 'adminconstructora/filtrar-por-nombre-edificio',
    FILTRAR_POR_NOMBRE_ENCARGADO = 'adminconstructora/filtrar-por-nombre-encargado',
    ASIGNAR_ENCARGADO = 'adminconstructora/asignar-encargado',
    DESASIGNAR_ENCARGADO = 'adminconstructora/desasignar-encargado',
    ELEGIR_CONSTRUCTORA = 'adminconstructora/elegir-constructora', // New endpoint
    TIENE_CONSTRUCTORA = 'adminconstructora/tiene-constructora', // New endpoint
    GET_CONSTRUCTORAS = 'adminconstructora/get-constructoras', // New endpoint
    EDIT_CONSTRUCTORA = 'adminconstructora/editar-constructora', // New endpoint
    
  }

  export enum AdminEndpoints {
    CREAR_ADMIN = 'administrador',
    GET_ADMIN_BY_EMAIL = 'administrador',
    INVITAR = 'administrador/invitar',
    ELIMINAR_INVITACION = 'administrador/invitar',
    ALTA_CATEGORIA = 'administrador/categoria',
    INVITACIONES_PARA_ELIMINAR = 'administrador/invitaciones',
  }

  export enum CategoriaEndpoints {
    GET_CATEGORIAS = 'categoria',
    GET_CATEGORIA_BY_NOMBRE = 'categoria',  // Placeholder for dynamic part
  }

  export enum DuenoEndpoints {
    GET_DUENO_BY_EMAIL = 'duenos',  // Placeholder for dynamic part
    INSERT_DUENO = 'duenos',
  }

  export enum ConstructoraEndpoints {
    GET_CONSTRUCTORAS = 'constructoras',
    GET_CONSTRUCTORA_BY_NOMBRE = 'constructoras', // Placeholder for dynamic part
    INSERT_CONSTRUCTORA = 'constructoras',
  }


  export enum EncargadoEndpoints {
    GET_ENCARGADOS = 'encargado',
    GET_CURRENT_ENCARGADO = 'encargado/current-encargado',
    CREAR_MANTENIMIENTO = 'encargado/mantenimiento',
    CREAR_SOLICITUD = 'encargado/solicitud',
    ASIGNAR_SOLICITUD = 'encargado/asignar',
    GET_SOLICITUD_BY_EDIFICIO = 'encargado/solicitudes/edificio',
    GET_SOLICITUD_BY_MANTENIMIENTO = 'encargado/solicitudes/mantenimiento',
    GET_TIEMPO_PROMEDIO_ATENCION = 'encargado/tiempo-promedio-atencion',
    GET_DUENO = 'encargado/Dueno',
    ASIGNAR_DUENO = 'encargado/asignar-dueno',
    GET_ALL_MANTENIMIENTOS = 'encargado/mantenimiento',
    GET_SOLICITUDES = 'encargado/solicitudes',
    DESASIGNAR_DUENO = 'encargado/desasignar-dueno',
    GET_ALL_SOLICITUDES = 'encargado/solicitudes/encargado',
  }

  export enum InvitacionEndpoints {
    INVITACION_ACCEPTED = 'invitacion',
  }
  
  export enum MantenimientoEndpoints {
    ATENDER_SOLICITUD = 'mantenimiento/atender',
    COMPLETAR_SOLICITUD = 'mantenimiento/completar',
    GET_SOLICITUDES = 'mantenimiento/solicitudes',
    GET_SOLICITUDES_ATENDIENDO = 'mantenimiento/solicitudes/atendiendo',
  }