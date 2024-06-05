export enum UsersEndpoints { 
    LOGIN = 'api/sessions',
    LOGOUT = 'api/sessions/logout',
}

export enum AdminConstructoraEndpoints {
    BORRAR_EDIFICIO = 'adminconstructora/borrar-edificio',
    GET_EDIFICIO = 'adminconstructora/get-edificio',
    GET_EDIFICIOS_POR_ADMIN = 'adminconstructora/get-edificiosporadmin',
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
  }

  export enum AdminEndpoints {
    CREAR_ADMIN = 'admin',
    GET_ADMIN_BY_EMAIL = 'admin',
    INVITAR = 'admin/invitar',
    ELIMINAR_INVITACION = 'admin/invitar',
    ALTA_CATEGORIA = 'admin/categoria',
  }

  export enum CategoriaEndpoints {
    GET_CATEGORIAS = 'categorias',
    GET_CATEGORIA_BY_NOMBRE = 'categorias',  // Placeholder for dynamic part
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
    GET_ENCARGADOS = 'encargados',
    GET_ENCARGADO_BY_EMAIL = 'encargados',
    CREAR_MANTENIMIENTO = 'encargados/mantenimiento',
    CREAR_SOLICITUD = 'encargados/solicitud',
    ASIGNAR_SOLICITUD = 'encargados/asignar',
    GET_SOLICITUD_BY_EDIFICIO = 'encargados/solicitudes/edificio',
    GET_SOLICITUD_BY_MANTENIMIENTO = 'encargados/solicitudes/mantenimiento',
    GET_TIEMPO_PROMEDIO_ATENCION = 'encargados/tiempo-promedio-atencion',
    GET_DUENO = 'encargados/Dueno',
    ASIGNAR_DUENO = 'encargados/asignar-dueno',
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