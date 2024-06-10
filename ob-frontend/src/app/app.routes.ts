import { Route } from '@angular/router';
import { ADMIN_URL, INVITACION_URL, ADMINCONSTRUCTORA_URL, ENCARGADO_URL, ENCARGADO_SOLICITUDES_URL, MANTENIMIENTO_URL, IMPORTAR_URL } from './utils/routes';
import { AdminComponent } from './components/admin/admin.component';
import { RoleGuard } from './guards/role.guard';
import { AuthGuard } from './guards/auth.guard';
import { MenuInvitacionesComponent } from './components/menu-invitaciones/menu-invitaciones.component';
import { AdminConstructoraComponent } from './components/admin-constructora/admin-constructora.component';
import { EncargadoComponent } from './components/encargado/encargado.component';
import { VisualizarSolicitudesComponent } from './components/visualizar-solicitudes/visualizar-solicitudes.component';
import { MantenimientoComponent } from './components/mantenimiento/mantenimiento.component';
import { ImportComponent } from './components/importar-edificios/importar-edificios.component';

const routes: Route[] = [
  {
    path: ADMIN_URL,
    component: AdminComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { expectedRole: 'admin' }
  },
  {
    path: INVITACION_URL,
    component: MenuInvitacionesComponent,
  },
  {
    path: ADMINCONSTRUCTORA_URL,
    component: AdminConstructoraComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { expectedRole: 'admin_constructora' }
  },
  {
    path: ENCARGADO_URL,
    component: EncargadoComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { expectedRole: 'encargado' }
  }, 
  {
    path: ENCARGADO_SOLICITUDES_URL,
    component: VisualizarSolicitudesComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { expectedRole: 'encargado' }
  },
  {
    path: MANTENIMIENTO_URL,
    component: MantenimientoComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { expectedRole: 'mantenimiento'}
  },
  {
    path: IMPORTAR_URL,
    component: ImportComponent,
    canActivate: [AuthGuard, RoleGuard],
    data: { expectedRole: 'admin_constructora' }
  }


];

export default routes;