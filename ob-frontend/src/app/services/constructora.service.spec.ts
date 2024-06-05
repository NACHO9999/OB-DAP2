import { TestBed } from '@angular/core/testing';

import { ConstructoraService } from './constructora.service';

describe('ConstructoraService', () => {
  let service: ConstructoraService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConstructoraService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
