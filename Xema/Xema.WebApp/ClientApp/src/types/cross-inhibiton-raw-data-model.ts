import { CrossInhibitonIndexCell } from './cross-inhibiton-index-cell';

export type Clusters = { [key: number]: string[] };

export interface CrossInhibitonRawDataModel {
  antigenLabels: string[][];
  markedAntigenLabels: string[];
  crossInhibitionIndexes: CrossInhibitonIndexCell[][][];
  clusters: Clusters
}