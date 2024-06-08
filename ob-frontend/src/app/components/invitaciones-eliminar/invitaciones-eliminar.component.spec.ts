import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitacionesEliminarComponent } from './invitaciones-eliminar.component';

describe('InvitacionesEliminarComponent', () => {
  let component: InvitacionesEliminarComponent;
  let fixture: ComponentFixture<InvitacionesEliminarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvitacionesEliminarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InvitacionesEliminarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
