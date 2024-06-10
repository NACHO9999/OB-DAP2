import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { SolicitudListItemComponent } from '../solicitud-list-item/solicitud-list-item.component';
import { MantenimientoService } from '../../services/mantenimiento.service';
import { ISolicitudModel } from '../../interfaces/isolicitud-model';
import { LogoutButtonComponent } from '../logout/logout.component';
import { take, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-mantenimiento',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    SolicitudListItemComponent,
    LogoutButtonComponent
  ],
  templateUrl: './mantenimiento.component.html',
  styleUrls: ['./mantenimiento.component.css']
})
export class MantenimientoComponent implements OnInit {
  solicitudesParaAtender: ISolicitudModel[] = [];
  solicitudesAtendiendo: ISolicitudModel[] = [];
  selectedSolicitudParaAtender: ISolicitudModel | null = null;
  selectedSolicitudAtendiendo: ISolicitudModel | null = null;

  constructor(private mantenimientoService: MantenimientoService) {}

  ngOnInit(): void {
    this.loadSolicitudesParaAtender();
    this.loadSolicitudesAtendiendo();
  }

  loadSolicitudesParaAtender(): void {
    this.mantenimientoService.getSolicitudesParaAtender().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getSolicitudesParaAtender:', err);
        return of([]);
      })
    ).subscribe((solicitudes: ISolicitudModel[]) => {
      this.solicitudesParaAtender = solicitudes;
    });
  }

  loadSolicitudesAtendiendo(): void {
    this.mantenimientoService.getSolicitudesAtendiendo().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getSolicitudesAtendiendo:', err);
        return of([]);
      })
    ).subscribe((solicitudes: ISolicitudModel[]) => {
      this.solicitudesAtendiendo = solicitudes;
    });
  }

  selectSolicitudParaAtender(solicitud: ISolicitudModel): void {
    this.selectedSolicitudParaAtender = solicitud;
  }

  selectSolicitudAtendiendo(solicitud: ISolicitudModel): void {
    this.selectedSolicitudAtendiendo = solicitud;
  }

  atenderSolicitud(): void {
    if (this.selectedSolicitudParaAtender) {
      this.mantenimientoService.atenderSolicitud(this.selectedSolicitudParaAtender.id).subscribe(() => {
        this.loadSolicitudesParaAtender();
        this.loadSolicitudesAtendiendo();
        this.selectedSolicitudParaAtender = null;
      });
    }
  }

  finalizarSolicitud(): void {
    if (this.selectedSolicitudAtendiendo) {
      this.mantenimientoService.completarSolicitud(this.selectedSolicitudAtendiendo.id).subscribe(() => {
        this.loadSolicitudesParaAtender();
        this.loadSolicitudesAtendiendo();
        this.selectedSolicitudAtendiendo = null;
      });
    }
  }
}
