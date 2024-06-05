import { TestBed } from '@angular/core/testing';

import { AdminConstructoraService } from './admin-constructora.service';

describe('AdminConstructoraService', () => {
  let service: AdminConstructoraService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdminConstructoraService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
