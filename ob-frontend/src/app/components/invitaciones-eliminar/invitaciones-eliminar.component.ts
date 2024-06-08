import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { IInvitacionModel } from '../../interfaces/iinvitacion-model';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-invitaciones-eliminar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './invitaciones-eliminar.component.html',
  styleUrls: ['./invitaciones-eliminar.component.css']
})
export class InvitacionesEliminarComponent implements OnInit {
  invitaciones: IInvitacionModel[] = [];

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.adminService.getInvitacionesParaEliminar().subscribe(
      (data: IInvitacionModel[]) => {
        this.invitaciones = data;
      },
      error => {
        console.error('Error al obtener invitaciones', error);
      }
    );
  }

  eliminarInvitacion(email: string) {
    this.adminService.eliminarInvitacion(email).subscribe(
      () => {
        console.log('Invitación eliminada');
        this.invitaciones = this.invitaciones.filter(inv => inv.email !== email);
      },
      error => {
        console.error('Error al eliminar invitación', error);
      }
    );
  }
}
