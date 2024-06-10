import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule, MatPseudoCheckbox } from '@angular/material/core';
import { MatRadioButton, MatRadioGroup } from '@angular/material/radio';
import { CommonModule } from '@angular/common';
import { AdminConstructoraService } from '../../services/admin-constructora.service';
import { EncargadoService } from '../../services/encargado.service';
import { IEdificioModel } from '../../interfaces/iedificio-model';
import { IConstructoraModel } from '../../interfaces/iconstructora-model';
import { IEncargadoModel } from '../../interfaces/iencargado-model';
import { catchError, finalize, of, take } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { LoadingService } from '../../services/loading.service';
import { EdificioListComponent } from '../edificio-list/edificio-list.component';
import { IDeptoModel } from '../../interfaces/idepto-model';
import { LogoutButtonComponent } from '../logout/logout.component';
import { DeptoListItemComponent } from '../depto-list-item/depto-list-item.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-constructora',
  templateUrl: './admin-constructora.component.html',
  styleUrls: ['./admin-constructora.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatOptionModule,
    MatRadioButton,
    MatRadioGroup,
    FormsModule,
    EdificioListComponent,
    LogoutButtonComponent,
    DeptoListItemComponent,
    MatCheckboxModule
  ],
})
export class AdminConstructoraComponent implements OnInit {
  createConstructoraForm: FormGroup;
  selectConstructoraForm: FormGroup;
  editConstructoraForm: FormGroup;
  edificioForm: FormGroup;
  deptoForm: FormGroup;
  asignarEncargadoForm: FormGroup;
  constructoras: IConstructoraModel[] = [];
  edificios: IEdificioModel[] = [];
  encargados: IEncargadoModel[] = [];
  tieneConstructora: boolean = false;
  selectedEdificio: IEdificioModel | null = null;
  selectedDepto: IDeptoModel | null = null;
  selectConstructora: boolean = false;
  conTerraza: boolean = false;
  edificioResultMessage: string = '';
  deptos: IDeptoModel[] = [];
  isEdificioSelected: boolean = false;
  buscaPorNombreEncargado: boolean = false;
  buscaConEncargado: boolean = false;
  buscaSinEncargado: boolean = false;
  nombreBusqueda: string = '';
  deptoResultMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private encargadoService: EncargadoService,
    private adminConstructoraService: AdminConstructoraService,
    private loadingService: LoadingService,
    private router: Router
  ) {
    this.createConstructoraForm = this.fb.group({
      nombreConstructora: ['', Validators.required]
    });

    this.selectConstructoraForm = this.fb.group({
      nombreConstructora: ['', Validators.required],
    });

    this.editConstructoraForm = this.fb.group({
      nombreConstructora: ['', Validators.required],
    });

    this.edificioForm = this.fb.group({
      nombre: ['', Validators.required],
      direccion: ['', Validators.required],
      ubicacion: ['', Validators.required],
      gastosComunes: [0, Validators.required]
    });

    this.asignarEncargadoForm = this.fb.group({
      encargado: [null, Validators.required]
    });

    this.deptoForm = this.fb.group({
      piso: ['', Validators.required],
      numero: ['', Validators.required],
      cantidadCuartos: ['', Validators.required],
      cantidadBanos: ['', Validators.required],
      conTerraza: [false, Validators.required]
    });

  }

  ngOnInit(): void {
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.adminConstructoraService.tieneConstructora().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in tieneConstructora:', err);
        return of(false);
      })
    ).subscribe((tiene: boolean) => {
      this.tieneConstructora = tiene;
      this.loadConstructoras();
      this.loadEdificios();
      this.loadEncargados();
    });
  }

  loadConstructoras(): void {
    this.adminConstructoraService.getConstructoras().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getConstructoras:', err);
        return of([]);
      })
    ).subscribe((constructoras: IConstructoraModel[]) => {
      this.constructoras = constructoras;
    });
  }

  loadEdificios(): void {
    this.adminConstructoraService.getEdificiosPorAdmin().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getEdificiosPorAdmin:', err);
        return of([]);
      })
    ).subscribe((edificios: IEdificioModel[]) => {
      this.edificios = edificios;
    });
  }

  loadEncargados(): void {
    this.encargadoService.getEncargados().pipe(
      take(1),
      catchError((err) => {
        console.error('Error in getEncargados:', err);
        return of([]);
      })
    ).subscribe((encargados: IEncargadoModel[]) => {
      this.encargados = encargados;
    });
  }

  selectExistingConstructora(): void {
    const selectedConstructora = this.selectConstructoraForm.get('nombreConstructora')?.value;
    if (selectedConstructora) {
      this.adminConstructoraService.elegirConstructora(selectedConstructora).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in elegirConstructora:', err);
          return of(null);
        })
      ).subscribe(() => {
        this.loadInitialData();
      });
    }
  }

  createNewConstructora(): void {
    const nombreConstructora = this.createConstructoraForm.get('nombreConstructora')?.value;
    if (nombreConstructora) {
      this.adminConstructoraService.crearConstructora(nombreConstructora).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in crearConstructora:', err);
          return of(null);
        })
      ).subscribe(() => {
        this.loadInitialData();
        console.log('Constructora creada');
      });
    }
  }

  createEdificio(): void {
    if (this.edificioForm.valid) {
      const edificio = this.edificioForm.value;
      edificio.deptos = [];
      this.adminConstructoraService.crearEdificio(edificio).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in crearEdificio:', err);
          this.edificioResultMessage = 'Error al crear edificio';
          return of(null);
        }),
        finalize(() => {
          // Stop loading here if using a loading indicator service
          this.loadingService.stopLoading();
        })
      ).subscribe((response: string) => {
        console.log('Response from crearEdificio:', response); // Log the response
        if (response) {
          this.loadEdificios();
        } else {
          this.edificioResultMessage = 'Error al crear edificio';
        }
      });
    } else {
      console.warn('Edificio form is invalid:', this.edificioForm.value); // Log invalid form data
    }
  }

  deleteEdificio(nombre: string, direccion: string): void {
    this.adminConstructoraService.borrarEdificio(nombre, direccion).pipe(
      take(1),
      catchError((err) => {
        console.error('Error in borrarEdificio:', err);
        return of(null);
      })
    ).subscribe(() => {
      this.loadEdificios();
      this.selectedEdificio = null;
      this.isEdificioSelected = false;
    });
  }

  selectEdificio(edificio: IEdificioModel): void {
    this.selectedEdificio = edificio;
    this.edificioForm.patchValue(edificio);
    this.deptos = edificio.deptos;
    console.log('depto:', this.deptos[0]);
    this.isEdificioSelected = true;
  }
  selectDepto(depto: IDeptoModel): void {
    this.selectedDepto = depto;
    this.deptoForm.patchValue(depto);
  }

  editEdificio(): void {
    if (this.edificioForm.valid && this.selectedEdificio) {
      const edificio = this.selectedEdificio;
      edificio.gastosComunes = this.edificioForm.value.gastosComunes;
      edificio.ubicacion = this.edificioForm.value.ubicacion;
      console.log('edificio a editar:', edificio);
      this.adminConstructoraService.editarEdificio(edificio).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in editarEdificio:', err);
          this.edificioResultMessage = 'Error al editar edificio';
          return of(null);
        })
      ).subscribe(() => {
        this.loadEdificios();
      });
    }
  }

  asignarEncargado(): void {
    if (this.asignarEncargadoForm.valid && this.selectedEdificio) {
      const emailEncargado = this.asignarEncargadoForm.get('encargado')?.value.email;
      console.log('emailEncargado:', emailEncargado);
      this.adminConstructoraService.asignarEncargado(emailEncargado, this.selectedEdificio.nombre, this.selectedEdificio.direccion).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in asignarEncargado:', err);
          return of(null);
        })
      ).subscribe(() => {
        this.loadEncargados();
      });
    }
  }

  desasignarEncargado(): void {
    if (this.selectedEdificio) {
      this.adminConstructoraService.desasignarEncargado(this.selectedEdificio.nombre, this.selectedEdificio.direccion).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in desasignarEncargado:', err);
          return of(null);
        })
      ).subscribe(() => {
        this.loadEncargados();
      });
    }
  }
  createDepto(): void {
    if (this.deptoForm.valid) {
      const depto: IDeptoModel = {
        piso: this.deptoForm.value.piso,
        numero: this.deptoForm.value.numero,
        cantidadCuartos: this.deptoForm.value.cantidadCuartos,
        cantidadBanos: this.deptoForm.value.cantidadBanos,
        conTerraza: this.deptoForm.value.conTerraza,
        edificioNombre: this.selectedEdificio?.nombre ?? '',
        edificioDireccion: this.selectedEdificio?.direccion ?? '',
        dueno: null  // Assuming this will be set later or is not needed here
      };

      this.adminConstructoraService.crearDepto(depto).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in crearDepto:', err);
          this.deptoResultMessage = 'Error al crear depto';
          return of(null);
        }),
        finalize(() => {
          this.loadingService.stopLoading();
        })
      ).subscribe((response: string) => {
        console.log('Response from crearDepto:', response);
        if (response) {
          this.loadEdificios();
          this.selectedEdificio?.deptos.push(depto);
          this.deptos = this.selectedEdificio?.deptos ?? [];
        } else {
          this.edificioResultMessage = 'Error al crear depto';
        }
      });
    } else {
      console.warn('Depto form is invalid:', this.deptoForm.value);
    }
  }
  editDepto(): void {
    if (this.deptoForm.valid && this.selectedDepto) {
      const depto: IDeptoModel = {
        piso: this.deptoForm.value.piso,
        numero: this.deptoForm.value.numero,
        cantidadCuartos: this.deptoForm.value.cantidadCuartos,
        cantidadBanos: this.deptoForm.value.cantidadBanos,
        conTerraza: this.deptoForm.value.conTerraza,
        edificioNombre: this.selectedDepto.edificioNombre,
        edificioDireccion: this.selectedDepto.edificioDireccion,
        dueno: this.selectedDepto.dueno  // Preserve existing owner if any
      };

      this.adminConstructoraService.editarDepto(depto).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in editarDepto:', err);
          this.deptoResultMessage = 'Error al editar depto';
          return of(null);
        }),
        finalize(() => {
          this.loadingService.stopLoading();
        })
      ).subscribe(() => {
        this.loadEdificios();
        if (this.selectedEdificio && this.selectedEdificio.deptos) {
          for (let i = 0; i < this.selectedEdificio.deptos.length; i++) {
            if (this.selectedEdificio.deptos[i].numero === depto.numero) {
              this.selectedEdificio.deptos[i] = depto;
              break;
            }
          }
          this.deptos = this.selectedEdificio.deptos;
        }
      });
    } else {
      console.warn('Depto form is invalid or no selected depto:', this.deptoForm.value);
    }
  }

  limpiarFiltros(): void {
    this.buscaConEncargado = false;
    this.buscaSinEncargado = false;
    this.nombreBusqueda = '';
    this.loadEdificios();
  }

  aplicarFiltros(): void {
    console.log('buscaConEncargado:', this.buscaConEncargado);
    console.log('buscaSinEncargado:', this.buscaSinEncargado);

    let edificiosObservable;

    if (this.buscaConEncargado && this.buscaSinEncargado) {
      edificiosObservable = this.adminConstructoraService.getEdificiosPorAdmin();
    } else if (this.buscaConEncargado) {
      edificiosObservable = this.adminConstructoraService.getEdificiosConEncargados();
    } else if (this.buscaSinEncargado) {
      edificiosObservable = this.adminConstructoraService.getEdificiosSinEncargados();
    } else {
      edificiosObservable = this.adminConstructoraService.getEdificiosPorAdmin();
    }

    edificiosObservable.pipe(
      take(1),
      catchError((err) => {
        console.error('Error in applying filters:', err);
        return of([]);
      })
    ).subscribe((edificios: IEdificioModel[]) => {
      this.edificios = edificios;
      this.applySearchFilter();
    });
  }


  applySearchFilter(): void {
    if (this.nombreBusqueda.trim()) {

      if (this.buscaPorNombreEncargado) {
        this.adminConstructoraService.filtrarPorNombreEncargado(this.nombreBusqueda).pipe(
          take(1),
          catchError((err) => {
            console.error('Error in filtrarPorNombreEncargado:', err);
            return of([]);
          })
        ).subscribe((edificios: IEdificioModel[]) => {
          this.edificios = edificios;
        });
      } else {
        this.edificios = this.edificios.filter((edificio: IEdificioModel) => {
          return edificio.nombre.toLowerCase().includes(this.nombreBusqueda.toLowerCase());
        });
      }
    }
  }

  editConstructora(): void {
    const nombreConstructora = this.editConstructoraForm.get('nombreConstructora')?.value;
    if (nombreConstructora) {
      this.adminConstructoraService.editarConstructora(nombreConstructora).pipe(
        take(1),
        catchError((err) => {
          console.error('Error in editarConstructora:', err);
          return of(null);
        })
      ).subscribe(() => {
        this.loadInitialData();
      });
    }
  }

  deleteDepto(numero: number, edificioNombre: string, edificioDireccion: string): void {
    this.adminConstructoraService.borrarDepto(numero, edificioNombre, edificioDireccion).pipe(
      take(1),
      catchError((err) => {
        console.error('Error in borrarDepto:', err);
        return of(null);
      })
    ).subscribe(() => {
      this.loadEdificios();
      if (this.selectedEdificio && this.selectedEdificio.deptos) {
        for (let i = 0; i < this.selectedEdificio.deptos.length; i++) {
          if (this.selectedEdificio.deptos[i].numero === numero) {
            this.selectedEdificio.deptos.splice(i, 1);
            break;
          }
        }
        this.deptos = this.selectedEdificio.deptos;
        this.selectedDepto = null;
      }
    });
  }
  navigateToImportar(): void {
    this.router.navigate(['admin_constructora/importar']);
  }
}
