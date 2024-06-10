import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MantenimientoItemListComponent } from './mantenimiento-item-list.component';

describe('MantenimientoItemListComponent', () => {
  let component: MantenimientoItemListComponent;
  let fixture: ComponentFixture<MantenimientoItemListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MantenimientoItemListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MantenimientoItemListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
