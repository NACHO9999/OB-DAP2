import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { AdminService } from '../../services/admin.service';
import { LogoutButtonComponent } from '../logout/logout.component';
import { InvitacionesEliminarComponent } from '../invitaciones-eliminar/invitaciones-eliminar.component';



@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
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
    LogoutButtonComponent,
    InvitacionesEliminarComponent
  ],
})
export class AdminComponent implements OnInit {
  inviteForm: FormGroup;
  adminForm: FormGroup;
  categoryForm: FormGroup;

  roles = [
    { value: 0, viewValue: 'Encargado' },
    { value: 1, viewValue: 'Administrador Constructora' },
  ];

  constructor(private fb: FormBuilder, private adminService: AdminService) {
    this.inviteForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      nombre: ['', Validators.required],
      rolInvitacion: [null, Validators.required],
      fechaExpiracion: ['', Validators.required],
    });

    this.adminForm = this.fb.group({
      Nombre: ['', Validators.required],
      Apellido: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Contrasena: ['', [Validators.required, Validators.minLength(6)]],
    });

    this.categoryForm = this.fb.group({
      Nombre: ['', Validators.required],
    });
  }

  ngOnInit(): void { 
    
  }
  dateFilter = (date: Date | null): boolean => {
    const currentDate = new Date();
    return (date || currentDate) > currentDate;
  };
  submitInvite() {

    if (this.inviteForm.valid) {
      console.log(this.inviteForm.value.RolInvitacion)
      const inviteData = this.inviteForm.value;
      this.adminService.invitar(inviteData).subscribe(
        response => {
          console.log('Invitación enviada', response);
          // Handle success response
        },
        error => {
          console.error('Error al enviar invitación', error);
          // Handle error response
        }
      );
    }
  }

  submitAdmin() {
    if (this.adminForm.valid) {
      const adminData = this.adminForm.value;
      this.adminService.crearAdmin(adminData).subscribe(
        response => {
          console.log('Admin creado', response);
        },
        error => {
          console.error('Error al crear admin', error);
        }
      );
    }
  }

  submitCategory() {
    if (this.categoryForm.valid) {
      const categoryData = this.categoryForm.value;
      this.adminService.altaCategoria(categoryData).subscribe(
        response => {
          console.log('Categoría creada', response);
          // Handle success response
        },
        error => {
          console.error('Error al crear categoría', error);
          // Handle error response
        }
      );
    }
  }
}