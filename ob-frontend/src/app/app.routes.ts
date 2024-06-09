import { Route } from '@angular/router';
import { ADMIN_URL, INVITACION_URL, ADMINCONSTRUCTORA_URL } from './utils/routes';
import { AdminComponent } from './components/admin/admin.component';
import { RoleGuard } from './guards/role.guard';
import { AuthGuard } from './guards/auth.guard';
import { MenuInvitacionesComponent } from './components/menu-invitaciones/menu-invitaciones.component';
import { AdminConstructoraComponent } from './components/admin-constructora/admin-constructora.component';

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

  // { path: PATHS.ADMINCONSTRUCTORA, component: AdminConstructoraComponent },
  // { path: PATHS.ADMIN, component: AdminComponent },
  // { path: PATHS.MANTENIMIENTO, component: MantenimientoComponent },
  // { path: PATHS.ENCARGADO, component: EncargadoComponent },
  // { path: PATHS.INVITACION, component: InvitacionComponent },
];

export default routes;