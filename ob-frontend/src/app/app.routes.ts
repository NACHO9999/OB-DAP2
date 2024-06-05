import { Route } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LOGIN_URL } from './utils/routes'; 

 const routes: Route[] = [
  { path: LOGIN_URL, component: LoginComponent }

  // { path: PATHS.ADMINCONSTRUCTORA, component: AdminConstructoraComponent },
  // { path: PATHS.ADMIN, component: AdminComponent },
  // { path: PATHS.MANTENIMIENTO, component: MantenimientoComponent },
  // { path: PATHS.ENCARGADO, component: EncargadoComponent },
  // { path: PATHS.INVITACION, component: InvitacionComponent },
];

export default routes;