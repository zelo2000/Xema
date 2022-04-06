import { CrossInhibitonIndexCell } from './cross-inhibiton-index-cell';

export interface CrossInhibitonRawDataModel {
  antigenLabels: string[];
  markedAntigenLabels: string[];
  crossInhibitionIndexes: (CrossInhibitonIndexCell[])[];
  clusters: { [key: number]: string[] }
}