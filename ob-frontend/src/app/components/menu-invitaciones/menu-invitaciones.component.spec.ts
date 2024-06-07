import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuInvitacionesComponent } from './menu-invitaciones.component';

describe('MenuInvitacionesComponent', () => {
  let component: MenuInvitacionesComponent;
  let fixture: ComponentFixture<MenuInvitacionesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MenuInvitacionesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MenuInvitacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
