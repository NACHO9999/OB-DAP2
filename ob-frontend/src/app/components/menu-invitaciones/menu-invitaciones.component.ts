import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { InvitacionService } from '../../services/invitacion.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu-invitaciones',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './menu-invitaciones.component.html',
  styleUrls: ['./menu-invitaciones.component.css']
})
export class MenuInvitacionesComponent implements OnInit {
  invitacionForm: FormGroup;
  resultMessage = '';
  funciono: boolean = true;

  constructor(
    private fb: FormBuilder,
    private invitacionService: InvitacionService,
    private router: Router
  ) {
    this.invitacionForm = this.fb.group({
      Email: ['', [Validators.required, Validators.email]],
      Contrasena: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void { }

  buscarInvitacion() {
    if (this.invitacionForm.valid) {
      const { Email, Contrasena } = this.invitacionForm.value;
      this.invitacionService.invitacionAccepted(Email, Contrasena).subscribe({
        next: () => {
          this.resultMessage = 'Invitación aceptada correctamente.';
          this.router.navigate(['/']);
        },
          complete: () => {
            this.resultMessage = 'Invitación no encotrada.';
          },
          error: (err) => {
            this.resultMessage = 'Error al aceptar la invitación.';
          }
        
      });
    } else {
      this.resultMessage = 'Por favor ingrese un email y contraseña válidos.';
    }
  }

  goToLogin() {
    this.router.navigate(['/']);
  }
}
