/* ------------------------------------------------------------------
 * Copyright (C) 2010 Martin Storsjo
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied.
 * See the License for the specific language governing permissions
 * and limitations under the License.
 * -------------------------------------------------------------------
 */

#ifndef VO_AMRWBENC_ENC_IF_H
#define VO_AMRWBENC_ENC_IF_H

#ifdef __cplusplus
extern "C" {
#endif

#ifndef AMRWB_WRAPPER_INTERNAL
    /* Copied from enc/src/gsmamr_enc.h */
    enum ModeWB {
        MRWB6 = 0,/* 6.6 kbps */
        MRWB8,    /* 8.85 kbps */
        MRWB12,     /* 12.56 kbps */
        MRWB14,     /* 14.25 kbps */
        MRWB15,     /* 15.85 kbps */
        MRWB19,    /* 19.85 kbps */
        MRWB23,    /* 23.05 kbps */
        MRWB24,    /* 23.85 kbps */
        MRWBDTX,    /* DTX       */
        MRWB_MODES   /* Not Used  */
    };
#endif
    
void* E_IF_init(void);
int E_IF_encode(void* state, int mode, const short* speech, unsigned char* out, int dtx);
void E_IF_exit(void* state);

#ifdef __cplusplus
}
#endif

#endif
