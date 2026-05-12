<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=!messagesPerField.existsError('username','password') displayInfo=realm.password && realm.registrationAllowed && !registrationDisabled??; section>
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
                <div class="login-brand-tagline">Sign in to your account</div>
            </div>

            <#if messagesPerField.existsError('username','password')>
            <div class="alert alert-error">
                <#if messagesPerField.existsError('username')>
                    ${messagesPerField.get('username')}
                <#elseif messagesPerField.existsError('password')>
                    ${messagesPerField.get('password')}
                </#if>
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

            <form id="kc-login-form" class="login-form" action="${url.loginAction}" method="post" novalidate>
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

                <div class="form-group">
                    <label for="password" class="form-label">${msg("password")}</label>
                    <input id="password"
                           class="form-input<#if messagesPerField.existsError('password')> error</#if>"
                           type="password"
                           name="password"
                           autocomplete="current-password"
                           placeholder="&bull;&bull;&bull;&bull;&bull;&bull;&bull;&bull;"
                           aria-invalid="<#if messagesPerField.existsError('password')>true</#if>"
                           dir="ltr" />
                    <#if messagesPerField.existsError('password')>
                    <span class="form-error-text" aria-live="polite">
                        ${kcSanitize(messagesPerField.get('password'))?no_esc}
                    </span>
                    </#if>
                </div>

                <#if realm.rememberMe && !usernameEditDisabled??>
                <div class="checkbox-group">
                    <#if login.rememberMe??>
                        <input id="rememberMe" class="checkbox-input" name="rememberMe" type="checkbox" checked tabindex="3" />
                    <#else>
                        <input id="rememberMe" class="checkbox-input" name="rememberMe" type="checkbox" tabindex="3" />
                    </#if>
                    <label for="rememberMe" class="checkbox-label">${msg("rememberMe")}</label>
                </div>
                </#if>

                <button type="submit" class="btn btn-primary btn-full">${msg("doLogIn")}</button>

                <#if realm.password && realm.resetPasswordAllowed>
                <div class="login-footer">
                    <span class="login-footer-text">
                        <a href="${url.loginResetCredentialsUrl}">${msg("doForgotPassword")}</a>
                    </span>
                </div>
                </#if>
            </form>

            <#if realm.password && social.providers?? && social.providers?has_content>
            <div class="social-section">
                <div class="social-divider">
                    <span class="social-divider-text">${msg("continueWith")}</span>
                </div>
                <div class="social-providers">
                    <#list social.providers as p>
                    <a href="${p.loginUrl}" class="social-provider-btn">
                        <#if p.iconClasses?has_content>
                            <img src="${url.resourcesPath}/${p.iconClasses?split(' ')?last?replace('fa-', '')?replace('icon-', '')}.svg"
                                 alt="${p.displayName}"
                                 onerror="this.style.display='none'" />
                        </#if>
                        ${p.displayName}
                    </a>
                    </#list>
                </div>
            </div>
            </#if>

            <#if realm.password && realm.registrationAllowed && !registrationDisabled??>
            <div class="login-footer">
                <span class="login-footer-text">
                    ${msg("noAccount")}
                    <a href="${url.registrationUrl}">${msg("doRegister")}</a>
                </span>
            </div>
            </#if>
        </div>
    </div>
    <#elseif section =="scripts">
    <script src="${url.resourcesPath}/js/anime.iife.min.js"></script>
    <script src="${url.resourcesPath}/js/login.js"></script>
    </#if>
</@layout.registrationLayout>