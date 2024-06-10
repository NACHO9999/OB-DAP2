import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { IUserCreate } from '../../interfaces/user-create';

@Component({
  selector: 'app-mantenimiento-item-list',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './mantenimiento-item-list.component.html',
  styleUrls: ['./mantenimiento-item-list.component.css']
})
export class MantenimientoListItemComponent {
  @Input() mantenimiento!: IUserCreate;
  @Output() select = new EventEmitter<IUserCreate>();

  onSelect() {
    this.select.emit(this.mantenimiento);
  }
}