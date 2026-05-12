<#import "template.ftl" as layout>
<@layout.registrationLayout displayInfo=false displayMessage=!messagesPerField.existsError('username'); section>
    <#if section =="form">
    <div class="login-background">
        <div class="login-gradient-base"></div>
        <div class="login-gradient-overlay"></div>
        <div class="login-gradient-spotlight"></div>
        <div class="login-particles" id="loginParticles"></div>
    </div>
    <svg class="cursor-circle" id="cursorCircle" viewBox="0 0 100 100">
        <circle cx="50" cy="50" r="0" fill="none" stroke="rgba(189, 187, 255, 0.6)" stroke-width="1.5" />
    </svg>
    <div class="login-container">
        <div class="login-card">
            <div class="login-brand">
                <div class="login-brand-tagline">${msg("emailForgotTitle")}</div>
            </div>

            <p class="reset-instruction">${msg("emailInstruction")}</p>

            <#if messagesPerField.existsError('username')>
            <div class="alert alert-error">
                ${messagesPerField.get('username')}
            </div>
            <#elseif message?has_content && message.type != 'warning'>
            <div class="alert alert-${message.type}">
                <#if message.type == 'error'>
                    ${kcSanitize(message.summary)?no_esc}
                <#else>
                    ${message.summary}
                </#if>
            </div>
            </#if>

            <form id="kc-reset-password-form" class="login-form" action="${url.loginAction}" method="post" novalidate>
                <div class="form-group">
                    <label for="username" class="form-label">
                        <#if !realm.loginWithEmailAllowed>${msg("username")}
                        <#elseif !realm.registrationEmailAsUsername>${msg("usernameOrEmail")}
                        <#else>${msg("email")}
                        </#if>
                    </label>
                    <input id="username"
                           class="form-input<#if messagesPerField.existsError('username')> error</#if>"
                           type="text"
                           name="username"
                           value="${login.username!' '}"
                           autofocus
                           autocomplete="username"
                           placeholder="${msg('username')}"
                           aria-invalid="<#if messagesPerField.existsError('username')>true</#if>"
                           dir="ltr" />
                    <#if messagesPerField.existsError('username')>
                    <span class="form-error-text" aria-live="polite">
                        ${kcSanitize(messagesPerField.get('username'))?no_esc}
                    </span>
                    </#if>
                </div>

                <button type="submit" class="btn btn-primary btn-full">${msg("doSubmit")}</button>

                <div class="login-footer">
                    <span class="login-footer-text">
                        <a href="${url.loginUrl}">${msg("backToLogin")}</a>
                    </span>
                </div>
            </form>
        </div>
    </div>
    <#elseif section =="scripts">
    <script src="${url.resourcesPath}/js/anime.iife.min.js"></script>
    <script src="${url.resourcesPath}/js/login.js"></script>
    </#if>
</@layout.registrationLayout>