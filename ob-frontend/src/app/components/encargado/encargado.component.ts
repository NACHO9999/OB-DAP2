import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { SolicitudListItemComponent } from '../solicitud-list-item/solicitud-list-item.component';
import { ISolicitudModel } from '../../interfaces/isolicitud-model';
import { ICategoriaModel } from '../../interfaces/icategoria-model';
import { IUserCreate } from '../../interfaces/user-create';
import { IDeptoModel } from '../../interfaces/idepto-model';
import { EncargadoService } from '../../services/encargado.service';
import { catchError, of, take } from 'rxjs';
import { CategoriaService } from '../../services/categoria.service';
import { v4 as uuidv4 } from 'uuid';
import { IEncargadoModel } from '../../interfaces/iencargado-model';
import { LogoutButtonComponent } from '../logout/logout.component';
import { DeptoListItemComponent } from '../depto-list-item/depto-list-item.component';
import { MatTableModule } from '@angular/material/table';
import { MantenimientoListItemComponent } from '../mantenimiento-item-list/mantenimiento-item-list.component';


@Component({
  selector: 'app-encargado',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    SolicitudListItemComponent,
    MatIconModule,
    LogoutButtonComponent,
    DeptoListItemComponent,
    MatTableModule,
    MantenimientoListItemComponent,
  ],
  templateUrl: './encargado.component.html',
  styleUrls: ['./encargado.component.css']
})
export class EncargadoComponent implements OnInit {
  solicitudForm: FormGroup;
  mantenimientoForm: FormGroup;
  solicitudes: ISolicitudModel[] = [];
  categorias: ICategoriaModel[] = [];
  mantenimientos: IUserCreate[] = [];
  selectedDepto: IDeptoModel | null = null;
  selectedSolicitud: ISolicitudModel | null = null;
  currentEncargado: IEncargadoModel | null = null;
  deptos: IDeptoModel[] = [];
  hidePassword: boolean = true;
  selectedMantenimiento: IUserCreate | null = null;
  displayedColumnsEdificio: string[] = ['edificio', 'abiertas', 'cerradas', 'atendiendo'];
  solicitudesEdificio: any[] = [];
  duenoForm: FormGroup;
  solicitudResultMessage: string = '';
  mantenimientoResultMessage: string = '';


  displayedColumnsMantenimiento: string[] = ['persona', 'abiertas', 'cerradas', 'atendiendo', 'tiempo'];
  solicitudesMantenimiento: any[] = [];

  constructor(private fb: FormBuilder, private encargadoService: EncargadoService, private categoriaService: CategoriaService, private router: Router) {
    this.solicitudForm = this.fb.group({
      descripcion: ['', Validators.required],
      categoria: ['', Validators.required],
      mantenimiento: [null]
    });
    this.mantenimientoForm = this.fb.group({
      nombre: ['', Validators.required],
      apellido: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contrasena: ['', [Validators.required, Validators.minLength(6)]]
    });
    this.duenoForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      nombre: ['', Validators.required],
      apellido: ['', Validators.required]
    });

  }
  createMantenimiento() {
    if (this.mantenimientoForm.valid) {
      const newMantenimiento: IUserCreate = this.mantenimientoForm.value;
      this.encargadoService.crearMantenimiento(newMantenimiento).subscribe({
        next: () => {
          this.loadMantenimientos();
          this.mantenimientoForm.reset();
        },
        error: (err) => {
          console.error('Error in createMantenimiento:', err);
          this.mantenimientoResultMessage = 'Error al crear el mantenimiento';
        }
      });
    }
  }


  ngOnInit(): void {

    this.loadCategorias();
    this.loadMantenimientos();
    this.getCurrentEncargado();
    this.loadSolicitudes();
  }
  loadSolicitudes(): void {
    this.encargadoService.getSolicitudes().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getSolicitudes:', err);
        return of([]);
      })
    ).subscribe((solicitudes: ISolicitudModel[]) => {
      this.solicitudes = solicitudes;
    });
  }

  asignarDueno() {
    if (this.duenoForm.valid && this.selectedDepto != null) {
      const newDueno = this.duenoForm.value;
      console.log('Payload:', JSON.stringify(newDueno, null, 2)); 
      this.encargadoService.asignarDueno(this.selectedDepto.numero, this.selectedDepto.edificioNombre, this.selectedDepto.edificioDireccion, newDueno).subscribe({
        next: () => {
          this.duenoForm.reset();
        },
        error: (err) => {
          console.error('Error in createDueno:', err);
        }
      });
    }
  }

  desasignarDueno(depto: IDeptoModel) {
    this.encargadoService.desasignarDueno(depto).subscribe({
      next: () => {
        console.log('Dueno desasignado');
        this.getCurrentEncargado();
        if (this.currentEncargado?.edificios) {
         for (let i = 0; i < (this.currentEncargado!.edificios.length ?? 0); i++) {
            for (let j = 0; j < (this.currentEncargado!.edificios[i].deptos?.length ?? 0); j++) {
              this.deptos.push(this.currentEncargado!.edificios[i].deptos[j]);
            }
          }
        }
      },
      error: (err) => {
        console.error('Error in desasignarDueno:', err);
      }
    });
  }


  loadCategorias(): void {
    this.categoriaService.getCategorias().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getCategorias:', err);
        return of([]);
      })
    ).subscribe((categorias: ICategoriaModel[]) => {
      this.categorias = categorias;
    });
  }
  loadMantenimientos(): void {
    this.encargadoService.getAllMantenimiento().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getMantenimientos:', err);
        return of([]);
      })
    ).subscribe((mantenimientos: IUserCreate[]) => {
      this.mantenimientos = mantenimientos;
    });
  }

  getCurrentEncargado(): void {
    this.encargadoService.getCurrentEncargado().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getCurrentEncargado:', err);
        return of(null);
      })
    ).subscribe((encargado: IEncargadoModel | null) => {
      this.currentEncargado = encargado;
      for (let i = 0; i < (this.currentEncargado!.edificios.length ?? 0); i++) {
        for (let j = 0; j < (this.currentEncargado!.edificios[i].deptos?.length ?? 0); j++) {
          this.deptos.push(this.currentEncargado!.edificios[i].deptos[j]);
        }
      }
    });
  }

  createSolicitud() {
    if (this.solicitudForm.valid && this.selectedDepto != null) {
      const newSolicitud: ISolicitudModel = {
        id: uuidv4().toUpperCase(),
        descripcion: this.solicitudForm.value.descripcion,
        categoria: this.categorias.find(c => c.nombre === this.solicitudForm.value.categoria)!,
        perMan: this.solicitudForm.value.mantenimiento,
        estado: 0,
        depto: this.selectedDepto,
        fechaInicio: new Date().toISOString(), // Ensure ISO string format
        fechaFin: null // Ensure ISO string format
      };
  
      console.log('Payload:', JSON.stringify(newSolicitud, null, 2)); // Log the payload
  
      this.encargadoService.crearSolicitud(newSolicitud).subscribe({
        next: () => {
          this.loadSolicitudes();
          this.solicitudForm.reset();
        },
        error: (err) => {
          console.error('Error in createSolicitud:', err);
          this.solicitudResultMessage = 'Error al crear la solicitud';
        }
      });
    }
  }


  selectDepto(depto: IDeptoModel): void {
    this.selectedDepto = depto;
    this.generateReportByEdificio();
  }
  generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  selectMantenimiento(mantenimiento: IUserCreate) {
    this.selectedMantenimiento = mantenimiento;
    this.generateReportByMantenimiento();
  }

  onAsignarSolicitud(): void {
    if (this.selectedMantenimiento && this.selectedSolicitud) {
      this.asignarSolicitud(this.selectedMantenimiento, this.selectedSolicitud);
    }
  }

  asignarSolicitud(mantenimiento: IUserCreate, solicitud: ISolicitudModel): void {
    this.encargadoService.asignarSolicitud(solicitud.id, mantenimiento.email).subscribe({
      next: () => {
        solicitud.perMan = mantenimiento;
        this.selectedSolicitud = null;
        this.loadSolicitudes();

      },
      error: (err) => {
        console.error('Error in asignarSolicitud:', err);
      }
    });
  }


  selectSolicitud(solicitud: ISolicitudModel) {
    this.selectedSolicitud = solicitud;
  }
  generateReportByEdificio(): void {
    if (this.selectedDepto != null) {
      this.encargadoService.getSolicitudByEdificio(this.selectedDepto.edificioNombre, this.selectedDepto.edificioDireccion).subscribe({
        next: (report: number[]) => {
          this.solicitudesEdificio = [{
            edificio: `${this.selectedDepto?.edificioNombre} - ${this.selectedDepto?.edificioDireccion}`,
            abiertas: report[0],
            cerradas: report[2],
            atendiendo: report[1]
          }];
        },
        error: (err) => {
          console.error('Error in generateReportByEdificio:', err);
        }
      });
    }
  }

  generateReportByMantenimiento(): void {
    if (this.selectedMantenimiento) {
      this.encargadoService.getSolicitudByMantenimiento(this.selectedMantenimiento.email).subscribe({
        next: (report: number[]) => {
          this.encargadoService.getTiempoPromedioAtencion(this.selectedMantenimiento!.email).subscribe({
            next: (tiempo: number | string) => {
              this.solicitudesMantenimiento = [{
                persona: `${this.selectedMantenimiento!.nombre} ${this.selectedMantenimiento!.apellido}`,
                abiertas: report[0],
                cerradas: report[2],
                atendiendo: report[1],
                tiempo: typeof tiempo === 'number' ? `${tiempo}h` : tiempo
              }];
            },
            error: (err) => {
              console.error('Error in getTiempoPromedioAtencion:', err);
            }
          });
        },
        error: (err) => {
          console.error('Error in generateReportByMantenimiento:', err);
        }
      });
    }
  }
  navigateToSolicitudes() {
    this.router.navigate(['/encargado/solicitudes']);
  }
}
