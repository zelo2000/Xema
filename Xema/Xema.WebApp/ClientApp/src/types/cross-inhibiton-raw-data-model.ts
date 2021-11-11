import { CrossInhibitonIndexCell } from './cross-inhibiton-index-cell';

export interface CrossInhibitonRawDataModel {
  antigenLabels: string[];
  markedAntigenLabels: string[];
  blankIndexes: number[];
  crossInhibitionIndexes: (CrossInhibitonIndexCell[])[];
}